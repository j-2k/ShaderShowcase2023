//incase i die trying to understand this while trying to make this shader ill just put a quick link to acerolas shell shader
//https://github.com/GarrettGunnell/Shell-Texturing/blob/main/Assets/Shell.shader
Shader "Unlit/ShellTextureShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color",color) = (0.2,0.8,0.4,1)
        _Distance("_Distance",float) = 0
        _SheetIndexNormalized("_SheetIndexNormalized",Range(0,1)) = 0

        _SheetIndex("_SheetIndex",int) = 0
        _SheetDensity("_SheetDensity",int) = 0
        _Thick("_Thick",float) = 0
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque"}
        Tags {"RenderType"="Opaque" "LightMode" = "ForwardBase" "Queue" = "Geometry"}
        LOD 100
        //Blend SrcAlpha OneMinusSrcAlpha
        //ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;

            float _Distance;
            float _SheetIndexNormalized;

            int _SheetIndex;
            int _SheetDensity;
            float _Thick;

            v2f vert (appdata v)
            {
                v2f o;
                

                
                v.vertex.xz += (sin(_Time.y + _SheetIndexNormalized)*_SheetIndexNormalized);
                
                //vertex & normal based scaling aka the true scale/offset method compared to my old hard coded quad y + offset
                v.vertex.xyz += v.normal.xyz * _Distance * _SheetIndexNormalized;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            float hash11(float p)
            {
                p = frac(p * .1031);
                p *= p + 33.33;
                p *= p + p;
                return frac(p);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                
                //resize uv
				float2 resizeUV = i.uv * 100;

                //frac(resizeUV) repeat uv 100 times, *2-1 makes it go from -1 to 1 (centering the UV), len takes the signed distance from the center making a circle(SDF),step just makes it 1 or 0 mainly done for colors
                float lenMask = 1 - length(frac(resizeUV) * 2-1);//1 - length(frac(resizeUV) * 2-1);

                //return lenMask;

                //yikes it took me a while to realize why it looked like this https://prnt.sc/qStIjm0B0Nxv instead of this blocky like version https://prnt.sc/jGhhiIbtCVhb
                //literally sat and looked at this garbage untill i realized it was a int holy sh i brainfarted so hard because i never used a int in shaders so i didnt look at the dt lmaooo
                uint2 intUV = resizeUV;

				uint seed = intUV.x + 100 * intUV.y + 100 * 10; 
                float rng = lerp(0,1,hash11(seed));
                
                //THICKNESS HANDLING
                //LENMASK IS INVERTED ABOVE MAKE SURE OF THE "1 -" & REMOVE IT IF NOT NEEDED

                /* acerola thickness
                int outsideThickness = (lenMask) > (2 * (rng - _SheetIndexNormalized));
                if(outsideThickness && _SheetIndex > 0) discard;
                */
                
                //my garbage thickness algorithm 
                //clip((lenMask * (1 - _Thick)) - ((_SheetIndexNormalized/rng) - _Thick));
                //below is an imitation of the above clip methiod but with discard, im doing it this way to just so its similar to acerolas thickness handler with discard
                int cone = ((lenMask * (1 - _Thick)) 
                - ((_SheetIndexNormalized/rng) - _Thick)) < 0;
                if(cone && _SheetIndex > 0) discard;

                /* old double if - changed to the 2 lines above.
                {
                    if(_SheetIndex == 0) return 0;//garbage way of just wanting a first black shell
                    if(rng > _SheetIndexNormalized)
                    {
                        //clipping dark areas
                        //clip((lenMask * (1 - _Thick)) - ((_SheetIndexNormalized/rng) - _Thick));
                        //return _Color * _SheetIndexNormalized;
                    }//hey this is something new i learned today, discard keyword discards the pixel so it doesnt render it i was just going to return 0? or clip? but this works
                    else{ discard; }
                }
                */
                
                //LIGHTING
                float dotNL = saturate(dot(i.normal, _WorldSpaceLightPos0)*1);  //dot between normal & light dir, standard lighting
                dotNL = dotNL * 0.5 + 0.5;  //same concept as (1-thick & (-thick))
                dotNL = dotNL * dotNL;      //valve square the last value, without doing this it its hard to see the difference vetween the lit and unlit areas

                //float FAOmul = _SheetIndexNormalized * 5;
                float FAOpow = pow(_SheetIndexNormalized, 2);

                return float4(_Color.xyz * FAOpow * dotNL,1);
            }
            ENDCG
        }
    }
}

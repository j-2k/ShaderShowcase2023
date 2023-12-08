// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//incase i die trying to understand this while trying to make this shader ill just put a quick link to acerolas shell shader
//https://github.com/GarrettGunnell/Shell-Texturing/blob/main/Assets/Shell.shader
Shader "Unlit/EmptyShellTextureShader"
{
    Properties
    {
        _SpherePosition("_SpherePosition",Vector) = (0,0,0,0)
        _MainTex ("Texture", 2D) = "white" {}
        _PerlinNoiseTex("_PerlinNoiseTex", 2D) = "white" {}
        _Color("Color",color) = (0.2,0.8,0.4,1)
        _Distance("_Distance",float) = 0
        _SheetIndexNormalized("_SheetIndexNormalized",Range(0,1)) = 0

        _SheetIndex("_SheetIndex",int) = 0
        _SheetDensity("_SheetDensity",int) = 0
        _Thick("_Thick",float) = 0

        _RNGceil("_RNGceil",float) = 1
        _RNGfloor("_RNGfloor",float) = 0
        _Size("Size",float) = 100

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

            sampler2D _PerlinNoiseTex; 
            float4 _PerlinNoiseTex_ST;

            float4 _Color;

            float _Distance;
            float _SheetIndexNormalized;

            int _SheetIndex;
            int _SheetDensity;
            float _Thick;

            float _RNGceil;
            float _RNGfloor;
            float _Size;

            float4 _SpherePosition;

            float hash11(float p)
            {
                p = frac(p * .1031);
                p *= p + 33.33;
                p *= p + p;
                return frac(p);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //default sway no random only sin, nvm brainfarted didnt need to add index offset, ignore the hash inthere was for test
                //float sway = sin(_Time.y + (_SheetIndexNormalized) * hash11(seed)) * _SheetIndexNormalized;
                float2 uvc = v.uv * 33;
                float seed = uvc.x + 100 * uvc.y + 100 * 10; 
                float sway = sin(_Time.y + (hash11(seed) + (_SheetIndexNormalized*0.3))) * _SheetIndexNormalized;
                float swayAmount = lerp(0,sway,0.2);
                float2 dir = float2(1,1) * 0;
                //v.vertex.xz += (dir * swayAmount);

                //sphere grass displacement
                //float3 worldPos = mul(unity_ObjectToWorld, v.vertex); //notneeded
                //https://gamedev.center/tutorial-how-to-make-an-interactive-grass-shader-in-unity/ YOINKED THIS
                
                float3 dirDisplacement = (v.vertex.xyz - _SpherePosition.xyz);
                float3 grassDisplace = normalize(dirDisplacement);
                //displacement vector is divided by radius of sphere to get a unclamped normal value that we then clamp and then inverse it to get the displacement
                float clampDisplacement = (saturate(length(dirDisplacement) / 1));
                grassDisplace *= (1.0 - clampDisplacement);//inverse the clamped displacement = if clamp disp == 0 => FULL DISPLACEMENT else if clamp disp == 1 => NO DISPLACEMENT
                grassDisplace *= 1.2; //scaling the strength of grass displacement
                
                //v.vertex.xyz += dirDisplacement;
                //v.vertex.xyz += normalize(dirDisplacement) * _SheetIndexNormalized;

                //v.vertex.y += (grassDisplace.y * 1.2) * _SheetIndexNormalized;
                //v.vertex.xz += (dir * swayAmount) + grassDisplace.xz * _SheetIndexNormalized;

                float3 grassDirection = v.vertex.xyz - _SpherePosition.xyz;
                float3 grassDisplacement = normalize(grassDirection) * (1.0 - saturate(length(grassDirection) / 1));
                //keeping y displacement will cause weird behavior sometimes so you can just remove it if you want
                //v.vertex.xz += grassDisplacement.xz * _SheetIndexNormalized; //example of above comment
                v.vertex.xyz += grassDisplacement.xyz * _SheetIndexNormalized;

                //v.vertex.xyz += grassDisplace * _SheetIndexNormalized;

                //vertex & normal based scaling aka the true scale/offset method compared to my old hard coded quad y + offset
                v.vertex.xyz += v.normal.xyz * _Distance * _SheetIndexNormalized;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }



            fixed4 frag (v2f i) : SV_Target
            {

                //resize uv
				float2 resizeUV = i.uv * _Size;
                //return float4(resizeUV.xy,0,1);
                
                //frac(resizeUV) repeat uv 100 times, *2-1 makes it go from -1 to 1 (centering the UV), len takes the signed distance from the center making a circle(SDF),step just makes it 1 or 0 mainly done for colors
                float lenMask = 1 - length(frac(resizeUV) * 2-1);//1 - length(frac(resizeUV) * 2-1);
                //return float4( 1 - length(frac(resizeUV.xy) * 2 - 1).xxx,1);
                //return lenMask;
                //return lenMask;

                //yikes it took me a while to realize why it looked like this https://prnt.sc/qStIjm0B0Nxv instead of this blocky like version https://prnt.sc/jGhhiIbtCVhb
                //literally sat and looked at this garbage untill i realized it was a int holy sh i brainfarted so hard because i never used a int in shaders so i didnt look at the dt lmaooo
                uint2 intUV = resizeUV;
				uint seed = intUV.x + 100 * intUV.y + 100 * 10; 
                float rng = lerp(_RNGfloor,_RNGceil,hash11(seed));


                //if(_SheetIndexNormalized<rng) return float4(0,1,0,1) * _SheetIndexNormalized;
                //else discard;
                //return 0;
                //return rng;

                //int acerolaCone = (1 - lenMask) > ((_Thick*5) * (rng - _SheetIndexNormalized));
                int cone = ((lenMask * (1 - _Thick )) - ((_SheetIndexNormalized/rng) - _Thick)) < 0;
                //int cone = lenMask < 0;
                //int cone = lenMask - (_SheetIndexNormalized/rng) < 0;
                if(cone && _SheetIndex > 0) discard;
                //return float4(_Color.xyz * _SheetIndexNormalized,1);
                //LIGHTING
                float dotNL = saturate(dot(i.normal, _WorldSpaceLightPos0)*1);  //dot between normal & light dir, standard lighting
                dotNL = dotNL * 0.5 + 0.5;  //same concept as (1-thick & (-thick))
                dotNL = dotNL * dotNL;      //valve square the last value, without doing this it its hard to see the difference vetween the lit and unlit areas

                //float FAOmul = _SheetIndexNormalized * 5;
                float FAOpow = pow(_SheetIndexNormalized, 1);

                return float4(_Color.xyz * FAOpow * dotNL,1);
            }
            ENDCG
        }
    }
}

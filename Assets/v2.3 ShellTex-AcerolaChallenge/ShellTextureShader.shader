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
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        Tags {"RenderType"="Opaque" "LightMode" = "ForwardBase"}
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

            v2f vert (appdata v)
            {
                v2f o;
                
                
                v.vertex.xyz += v.normal.xyz * _Distance * _SheetIndexNormalized;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
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
				float2 resizeUV = i.uv * 100;

                //yikes it took me a while to realize why it looked like this https://prnt.sc/qStIjm0B0Nxv instead of this blocky like version https://prnt.sc/jGhhiIbtCVhb
                //literally sat and looked at this garbage untill i realized it was a int holy sh i brainfarted so hard because i never used a int in shaders so i didnt look at the dt lmaooo
                uint2 intUV = resizeUV;
                //uint seedGen = intUV.x + 100 * intUV.y;
				uint seed = intUV.x + 100 * intUV.y + 100 * 10; 
                
                //return float4(0,0.5*hash11(seedGen),0,1);
                
                float rng = hash11(seed);

                if(rng > _SheetIndexNormalized)
                {
                    return _Color * _SheetIndexNormalized;
                    //return float4(0,1* _SheetIndexNormalized,0,1); 
                }
                else
                {
                    discard; //hey this is something new i learned today, discard keyword discards the pixel so it doesnt render it i was just going to return 0? or clip? but this works
                }

                return _Color;
                
                //return _Color;
                //return col;
            }
            ENDCG
        }
    }
}

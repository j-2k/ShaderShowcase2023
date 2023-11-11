//incase i die trying to understand this while trying to make this shader ill just put a quick link to acerolas shell shader
//https://github.com/GarrettGunnell/Shell-Texturing/blob/main/Assets/Shell.shader
Shader "Unlit/ShellTextureShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color",color) = (0.2,0.8,0.4,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
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
				float2 resizeUV = i.uv*100;

                //yikes it took me a while to realize why it looked like this https://prnt.sc/qStIjm0B0Nxv instead of this blocky like version https://prnt.sc/jGhhiIbtCVhb
                //literally sat and looked at this garbage untill i realized it was a int holy sh i brainfarted so hard because i never used a int in shaders so i didnt look at the dt lmaooo
                uint2 intUV = resizeUV;
                uint seedGen = intUV.x + 100 * intUV.y;
				//uint seed = intUV.x + 100 * intUV.y + 100 * 10; 
                
                return float4(0,0.5*hash11(seedGen),0,1);
                /*
                float rng = hash11(i.uv.x);

                if(rng > 0)
                {
                    return float4(0,1,0,1);
                }
                else
                {
                    return float4(0,0,0,1);
                }*/
                
                //return _Color;
                //return col;
            }
            ENDCG
        }
    }
}

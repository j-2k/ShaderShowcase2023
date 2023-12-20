Shader "Unlit/ShiftSquares"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex2 ("Texture2", 2D) = "white" {}
        _Size("UV Size", Range(0, 30)) = 10
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _MainTex2;
            float4 _MainTex2_ST;

            float _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float hash11(float p)
            {
                p = frac(p * .1031);
                p *= p + 33.33;
                p *= p + p;
                return frac(p);
            }

            float hash12(float2 p)
            {
                float3 p3  = frac(float3(p.xyx) * .1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }


            fixed4 frag (v2f i) : SV_Target
            {
                //return sin(_Time.y);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_MainTex2, i.uv);
                //return col2;

                float2 uv = floor(i.uv * _Size);
                float seed =(uv.x*32) + (uv.y + 8);
                float r = hash11(seed + sin(_Time.x*0.03));
                float s = sin(_Time.y*2);
                //float g = smoothstep(-0.5,0.5,sin(_Time.y*1));//gradient
                //float as = abs(s)-1;
                if(s < 0)
                {   
                    r = lerp(r+s,1,col*1);
                    //r += s + (col+r);
                    clip(r-0.01);
                    //return float4(1,0,1,1)*r;
                }
                else
                {

                }
                return float4(1,0,1,1)*r;

                /*
                float s = step(0,sin(_Time.y));
                //float s = min(max(sin(_Time.y)*3, 0), 1);
                float ll = lerp(rng,1-rng,s);

                //https://graphtoy.com/
                //GRADIENT FOUND VIA SMOOTHSTEP AKA THE TRANSITION SLOPE
                float gradient = smoothstep(-0.5,0.5,sin(_Time.y*1));
                //rng += sin(_Time.y);
                float gMAX = lerp(0,rng,gradient);
                return rng;
                */
            }
            ENDCG
        }
    }
}

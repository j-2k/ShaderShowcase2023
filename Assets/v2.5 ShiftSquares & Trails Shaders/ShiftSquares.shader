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

            sampler2D _MainTex2;
            float4 _MainTex2_ST;

            float _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
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
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                //float2 uv = frac(i.uv * 10);
                float2 uv = floor(i.uv * _Size);
                float r = hash11((uv.x*32) + (uv.y+8));
                //float r = hash12(uv.xy);
                float3 fc = float3(r.xxx + sin(_Time.y));
                float s = step(0,sin(_Time.y));
                //float s = min(max(sin(_Time.y)*3, 0), 1);
                float ll = lerp(fc,1-fc,s);

                //https://graphtoy.com/
                //GRADIENT FOUND VIA SMOOTHSTEP AKA THE TRANSITION SLOPE
                float gradient = smoothstep(-0.2,0.2,sin(_Time.y*1));
                float gMAX = lerp(fc,1,gradient);
                float gMID = lerp(0,fc,gMAX);
                //gMID = lerp(fc,1 - fc,gradient);
                //float gMID = lerp(col,fc,1 - gMAX);//care inverse of interpolater var t here
                //float gMIN = lerp(col2,col,gradient);
                //clip(gMID - 0.01);
                return gradient;
                //return fc.x;


                //float l = lerp(col,1,s);//lerp(col,fc,s);
                //float l2 = lerp(0,fc,l);//lerp(col,fc,s);
                //clip(l2-0.01);
                //return l2;
                //fc = saturate(fc);
                clip(ll - 0.01);
                return float4(ll.xxx,1);
            }
            ENDCG
        }
    }
}

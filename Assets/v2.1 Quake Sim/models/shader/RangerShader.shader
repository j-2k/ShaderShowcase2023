Shader "Unlit/RangerShader"
{
    Properties
    {
        _MainTex ("MASK TEX", 2D) = "white" {}
        _ColTex ("COLORED TEX", 2D) = "white" {}
        _Color("Color", color) = (0,0,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
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

            sampler2D _ColTex;
            float4 _ColTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float3 palette( in float t, in float3 a, in float3 b, in float3 c, in float3 d )
            {
                return a + b*cos( 6.28318*(c*t+d) );
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 maskTex = 1 - (1 - tex2D(_MainTex, i.uv));
                fixed4 colTex = (tex2D(_ColTex, i.uv));
                // apply fog
                /*
                //starTexCol *= _Color;
                //float4 fc = lerp(col,starTexCol,col);
                UNITY_APPLY_FOG(i.fogCoord, fc);
                //float4 fc = abs(sin(i.uv.x + _Time.y*2)) * 10;

                return col + starTexCol;
                */
                float2 uv = i.uv * 2 -1;

                float2 nuv = i.uv;

                uv.x += 0.41;
                uv.y -= 0.1;
                
                float3 fracCol = float3(0,0,0);
                
                //uv.x += 100;
                for (float i = 0.0; i < 3.0; i++)
                {
                    uv = frac(uv*1.5) - 0.5;

                    float d = length(uv); // / exp(length(nuv));
                    
                    float3 col = palette(i * 0.5 + _Time.y * 0.5 + length(uv),float3(0.5, 0.5, 1.5),float3(1.5, 0.5, 1.5),float3(1.0, 1.0, 1.0),float3(0.00, 0.33, 0.67));
                    
                    d = abs(sin(d * 20.0 + _Time.y) / 1);
                    
                    d = 0.01/d;
                    
                    //col *= d;
                    fracCol += col * d;
                }
                
                //return float4(float3(fc),1);
    

                //

                float4 finalCol = float4(float3(fracCol),1);
                
                return lerp(colTex,finalCol,maskTex);

                /*
                if(maskTex.x > 0.01f)
                {
                    return finalCol;
                }
                else
                {
                    return colTex;
                }
                */
                
                
            }
            ENDCG
        }

    }
    FallBack "Diffuse"
}

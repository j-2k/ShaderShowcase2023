Shader "Unlit/JumpPadULS"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            float3 palette( in float t, in float3 a, in float3 b, in float3 c, in float3 d )
            {
                return a + b*cos( 6.28318*(c*t+d) );
            }

            float sdBox( in float2 p, in float2 b )
            {
                float2 d = abs(p)-b;
                return length(max(d,0.0)) + min(max(d.x,d.y),0.0);
            }

            float sdEquilateralTriangle( in float2 p, in float r )
            {
                const float k = sqrt(3.0);
                p.x = abs(p.x) - r;
                p.y = p.y + r/k;
                if( p.x+k*p.y>0.0 ) p = float2(p.x-k*p.y,-k*p.x-p.y)/2.0;
                p.x -= clamp( p.x, -2.0*r, 0.0 );
                return -length(p)*sign(p.y);
            }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float2 rotateUV(in float2 uv, float theta)
            {

                float u = (uv.x * cos(theta)) - uv.y * sin(theta);
                float v = (uv.x * sin(theta)) + uv.y * cos(theta);

                return float2(u,v);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                /* DOTS1
                float2 uv = i.uv * 20;
                float dots = sin(uv.y + _Time.y) * sin(uv.x + _Time.y);
                dots = lerp(-1,2,dots);
                */
                //DOTS2
                /*
                float2 uvc = i.uv * 2 - 1;
                float2 uv1 = rotateUV(uvc,3);
                return float4(uv1,0,1);
                */

                float2 uv = i.uv * 2 - 1;
                uv = rotateUV(uv,_Time.y*0.2);
                
                float3 cCol = float3(0,0,0);
                float3 fc = float4(0,0,0,1);
                for (float i = 0; i<3; i++)
                {
                    //float c = length(uv);
                    float b = sdBox(uv,1);
                    uv = frac(uv * 1.5) - 0.5;
                    cCol = palette(i * 0.7 + _Time.y * 0.5 + b,float3(0.5, 0.5, 0.5),float3(0.5, 0.5, 0.5),float3(1.0, 1.0, 1.0),float3(0.00, 0.10, 0.20));
                    b = abs(sin(b * 10.0 + _Time.y));
                    //b = 0.1/b;
                    b = 0.05/b;

                    fc += cCol * b;
                }
                
                return saturate(float4(fc,1));
            }
            ENDCG
        }
    }
}

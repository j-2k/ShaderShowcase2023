Shader "Unlit/WallColors"
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 nuv = i.uv * 3 + _Time.y * 10;
                float3 cp1 = float3(palette(_Time.y*0.5,float3(0.5,0.5,0.5),float3(0.5,0.5,0.5),float3(1,1,1),float3(0,0.33,0.67)).rgb);
                float3 cp2 = float3(palette(_Time.y*0.5 + 0.5,float3(0.5,0.5,0.5),float3(0.7,0.5,0.3),float3(0.1,0.5,0.9),float3(0.5,0.2,0.9)));

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float3 fc = lerp(cp1,cp2,sin(nuv.y));
                return float4(fc,1);
            }
            ENDCG
        }
    }
}

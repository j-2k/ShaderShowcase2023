Shader "Unlit/RangerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StarTex ("StarTex", 2D) = "white" {}
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

            sampler2D _StarTex;
            float4 _StarTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float3 palette( float t ) {
                float3 a = float3(0.5, 0.5, 0.5);
                float3 b = float3(0.5, 0.5, 0.5);
                float3 c = float3(1.0, 1.0, 1.0);
                float3 d = float3(0.263,0.416,0.557);

                return a + b*cos( 6.28318*(c*t+d) );
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = 1 - (1 - tex2D(_MainTex, i.uv));
                // apply fog
                float2 starUV = (i.uv * 1) + _Time.y * 0;
                fixed4 starTexCol = (tex2D(_StarTex, starUV));
                //starTexCol *= _Color;
                //float4 fc = lerp(col,starTexCol,col);
                UNITY_APPLY_FOG(i.fogCoord, fc);
                //float4 fc = abs(sin(i.uv.x + _Time.y*2)) * 10;

                return col + starTexCol;

                //
                /*

                if(col.x > 0.01f)
                {
                    return float4(finalColor,1);
                }
                else
                {
                    return starTexCol;
                }
                */
            }
            ENDCG
        }

    }
    FallBack "Diffuse"
}

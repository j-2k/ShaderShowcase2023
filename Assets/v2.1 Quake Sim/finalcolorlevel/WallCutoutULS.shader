Shader "Unlit/WallCutoutULS"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TimeScale ("Time Scale", float) = 5
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
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TimeScale;

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
                float2 uv = i.uv + _Time.y * _TimeScale;

                float uv1 = sin(uv.y * 2);
                float uv2 = sin(uv.y * 2 + 3.5);

                //float3 cp = float3(palette(_Time.y,float3(0.5,0.5,0.5),float3(0.5,0.5,0.5),float3(1,1,1),float3(0,0.33,0.67)).rgb);
                
                float mc1 = uv1;
                mc1 = saturate(mc1);

                float mc2 = uv2;
                mc2 = saturate(mc2);

                //float3 finalColor = float3(mainCol * cp.x, mainCol * cp.y, mainCol * cp.z);
                float3 fc1 = (mc1 * palette(_Time.y,float3(0.5,0.5,0.5),float3(0.5,0.5,0.5),float3(1,1,1),float3(0,0.33,0.67)));
                float3 fc2 = (mc2 * palette(_Time.y + 0.5,float3(0.5,0.5,0.5),float3(0.5,0.5,0.5),float3(1,1,1),float3(0,0.33,0.67)));

                //return fc;
                return float4(fc1 + fc2 ,1);
                /*
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
                */
            }
            ENDCG
        }
    }
}

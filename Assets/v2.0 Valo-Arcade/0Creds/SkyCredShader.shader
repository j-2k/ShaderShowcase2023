Shader "Unlit/SkyCredShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColU("Color UP", color) = (0,1,0,1)
        _ColD("Color DOWN", color) = (1,0,1,1)
        _R1("R1", float) = 0
        _R2("R2", float) = 1
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

            float4 _ColD;
            float4 _ColU;
            float _Offset;
            float _R1;
            float _R2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float InverseLerp(float a, float b, float t)
            {
                return (t-a)/(b-a);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float iLerp = saturate(InverseLerp(_R1,_R2, i.uv.y));
                float4 fc = (lerp(_ColD,_ColU,iLerp));
                return fc;
            }
            ENDCG
        }
    }
}

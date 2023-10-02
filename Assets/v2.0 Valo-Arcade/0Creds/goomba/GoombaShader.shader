Shader "Unlit/GoombaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Col("Color", color) = (0,1,0,1)
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
            fixed4 _Col;

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
                fixed4 star = tex2D(_MainTex, i.uv);
                clip(star.a - 0.01);
                fixed4 outlineStar = 1 - star;
                fixed4 fStar = star * _Col;



                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, fStar);
                return fStar;
            }
            ENDCG
        }
    }
}

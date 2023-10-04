Shader "Unlit/GoombaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Col("Color", color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Back
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;

            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
                float4 Custom1 : TEXCOORD1;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Col;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
                o.Custom1 = float4(v.uv.zw,v.texcoord1.xy);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 star = tex2D(_MainTex, i.uv);
                clip(star.a - 0.01);
                fixed4 outlineStar = (1 - star) * 7;//7 is the glow of the star outline this addition was huge since now it blends with the all white outlines on the dance shaders
                fixed4 fStar = star * _Col * i.Custom1 + outlineStar;
                


                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, fStar);
                return fStar;
            }
            ENDCG
        }
    }
}

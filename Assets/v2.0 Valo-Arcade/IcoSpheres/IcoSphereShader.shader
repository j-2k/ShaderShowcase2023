Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Emission ("Fresnel Emission Color", color) = (1,1,1,1)
        _FresnelExponent ("Fresnel Exponent", float ) = 0.2
        _Color ("Base Col", color) = (1,1,1,1)
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 wNormal : TEXCOORD2;
                float3 viewDir : POSITION1;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Emission;
            float _FresnelExponent;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.wNormal = (v.normal);
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                                float f = dot(i.wNormal,i.viewDir);
                f = saturate(f);
                f = pow(f, _FresnelExponent);

                float3 fc = (_Emission.xyz * f);
                return float4(fc,1) + _Color;
            }
            ENDCG
        }
    }
}

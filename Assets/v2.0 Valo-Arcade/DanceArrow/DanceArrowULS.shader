Shader "Unlit/DanceArrowULS"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Primary Color", color) = (0.5,0,1,1)
        _Color2 ("Secondary Color", color) = (1,1,1,1)
        [HDR] _Emission ("Fresnel Emission Color", color) = (1,1,1,1)
        _FresnelExponent ("Fresnel Exponent", float ) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"  "Queue" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
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
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 wNormal : TEXCOORD2;
                float3 viewDir : POSITION1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color1;
            float4 _Color2;
            float4 _Emission;

            float _FresnelExponent;

            v2f vert (appdata v)
            {
                v2f o;
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.wNormal = normalize((v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 angleCol = tex2D(_MainTex, i.uv);
                fixed4 flatCol = 1 - tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);

                float f = dot(i.wNormal,i.viewDir);
                f = saturate(1 - f);
                f = pow(f, _FresnelExponent);

                float4 fresnelCol = float4((_Emission.xyz * f),1);

                float4 fCol = (flatCol * _Color2) + (angleCol * _Color1) + fresnelCol;
                return float4(fCol.xyz,0.8);
            }
            ENDCG
        }
    }
}

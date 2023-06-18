Shader "Unlit/RangerAfterglow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Emission ("Fresnel Emission Color", color) = (1,1,1,1)
        _FresnelExponent ("Fresnel Exponent", float ) = 0.2
        _FresnelOffset ("_FresnelOffset", Range(0,10) ) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        ZWrite On
        Cull Back
        //ZTest
        //ZClip True
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
                float3 worldNormal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Emission;
            float _FresnelExponent;
            float _FresnelOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                //o.worldNormal = (v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float f = dot(i.worldNormal,i.viewDir) - _FresnelOffset;
                f = saturate(f);
                f = pow(f, abs(_FresnelExponent));

                float4 fc = 1 - (f);

                //return float4(fc,1) + _Color;

                return float4(_Emission.xyz * fc.xyz, fc.w);
            }
            ENDCG
        }


    }
}

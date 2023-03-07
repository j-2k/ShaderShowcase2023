Shader "Unlit/FloorGrid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Color ("Color", color) = (1,1,1,1)
        _LineThickness ("Line Thickness", float) = 100
        _LineDensity ("Line Density", float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry"}
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
                float3 normals : NORMAL;
                float3 viewDir : POSITION1;
                float3 worldNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _LineDensity;
            float _LineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                o.normals = v.normal;
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float x = 1 - smoothstep(2,0, cos(i.uv.x * 6.28 * _LineDensity)+1)*pow(_LineThickness,2);
                float y = 1 - smoothstep(2,0, cos(i.uv.y * 6.28 * _LineDensity)+1)*pow(_LineThickness,2);
                float fxy = smoothstep(2,1,(x*y));
                //float dotVN = smoothstep(2,3,3 * dot(i.viewDir,i.worldNormal));
                //float4 fxy = float4(i.worldNormal,1);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, _Color);

                return float4(fxy * _Color);
                //return float4((fxy * dotVN) * _Color);
            }
            ENDCG
        }
    }
    Fallback "Standard"
}

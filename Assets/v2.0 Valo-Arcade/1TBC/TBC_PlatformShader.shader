Shader "Unlit/TBC_PlatformShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SqMask ("Square Mask", 2D) = "white" {}
        [HDR] _Em("Emission", color) = (1,1,1,1)
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
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _SqMask;
            float4 _SqMask_ST;

            float4 _Em;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.normal = (v.normal);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 onlyY = lerp(1,0,(i.normal.y));
                float3 topSq = 1 - tex2D(_SqMask, i.uv).rgb;
                //0,1,0
                /*i wasted like an hr when i found out i just had to add brain fart moment
                topSq *= 1 - onlyY;
                topSq = saturate(topSq);
                float3 topMat = saturate((1 - topSq - onlyY) * col);
                float3 finalPlat = ((topMat) + (onlyY + topSq) * _Em);
                */
                    
                float3 fPlat = saturate(onlyY + col + topSq) * _Em;
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, fPlat);
                return float4(fPlat,1);
            }
            ENDCG
        }
    }
}

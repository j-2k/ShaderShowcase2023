Shader "Unlit/SDFBasicCurve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                float2 uvc = i.uv;
                uvc.x *= 4;
                uvc = uvc * 2 - 1;

                //uvc.x -= 3;
                float2 midl = float2(clamp(uvc.x,0,6),0);
                float2 l = length(uvc);

                float sdf = distance(uvc,midl) * 2 - 2;
                clip(-sdf);

                float borderSDF = sdf + 0.5;
                //float borderSDF = step(0,sdf + 0.5);

                float pd = fwidth(borderSDF);
                float AA_Border = saturate(borderSDF/pd);

                //return float4(borderSDF.xxx,1);
                return float4(AA_Border.xxx,1);
            }
            ENDCG
        }
    }
}

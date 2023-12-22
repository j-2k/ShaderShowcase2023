Shader "Unlit/RectTrail"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);

                //https://developer.download.nvidia.com/cg/sin.html
                //i thought sin was optimized holy shit its a mess in there i will use length instead
                //float2 uv = 1 - i.uv;
                //return float4(uv.xxxx * sin(uv.y*3.14));

                //circle sdf implementation
                /*
                float2 uv = float2(i.uv.x*0.5,i.uv.y-0.5)*2;
                float d = saturate(1-length(uv));
                float3 l = lerp(float3(1,1,0) * d,float3(1,0,1) * d,sin(_Time.y*2));
                return float4(l*2,d);
                */

                //tex implementation
                return ((sin(_Time.w)*0.25 + 0.25) * col.x) + col.x;
            }
            ENDCG
        }
    }
}

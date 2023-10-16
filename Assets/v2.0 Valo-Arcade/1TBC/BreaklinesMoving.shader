Shader "Unlit/BreaklinesMoving"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Col("Color", color) = (0.1,1,1,1)
        [HDR] _Em("Emission", color) = (0.1,1,1,1)
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
            
            float4 _Col;
            float4 _Em;

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
                //UNITY_APPLY_FOG(i.fogCoord, col);
                /*
                float l = saturate(sin(i.uv.x * 10 + _Time.y));
                l += (1-col.x);
                l = saturate(l);
                clip(l- 0.1);
                */  

                float2 uvc = i.uv * 2 - 1;

                float l = smoothstep(0,0.4,uvc.y - 0.6);
                float l2 = (sin(uvc.y * 10 + _Time.y * 2) * (1 - l)) * abs(sin(_Time.y* 3));
                float4 fc = (l2 + l);
                float yMask = lerp(0,0.5,uvc.y + 1);
                fc = saturate(fc) * (yMask * 2);
                clip(fc.a - 0.01);
                fc *= _Col * _Em;
                UNITY_APPLY_FOG(i.fogCoord, fc);

                return fc; 
            }
            ENDCG
        }
    }
}

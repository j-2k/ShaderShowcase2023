Shader "Unlit/DDRArrowFloor"
{
    Properties
    {
        _MainTex ("Hexagon Texture", 2D) = "white" {}
        [HDR] _Color ("Emission Color / W IS ALPHA CONTROL", color) = (0.83,0,1,0.5)
        _ArrowStr("Arrow Strength", Range(0,2)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;

            float _ArrowStr;

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
                //float2 uv = i.uv * 1.5 - 0.25;//* sin(_Time.y);
                float2 uv = i.uv * (sin(_Time.y * 2) * 0.25 + 1.25) - (sin(_Time.y*2) * 0.125 + 0.125);
                fixed4 col = tex2D(_MainTex, uv);
                clip(col.z - 0.1);

                //col = saturate(smoothstep(0,1,col) + 2);
                //col = smoothstep(0,1,col.x); //black white arrow
                float l = saturate(1 - length(i.uv - 0.5) * 4);
                col = smoothstep(0,1,col);

                


                //float4 fc = float4(l2,1);
                //return fc;
                return float4(l.xxx + col.xyz,_Color.w);
            }
            ENDCG
        }
    }
}

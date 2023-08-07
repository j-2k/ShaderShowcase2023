Shader "Unlit/ValDanceFloorULS"
{
    Properties
    {
        _MainTex ("Hexagon Texture", 2D) = "white" {}
        _EmTex ("Emission Texture", 2D) = "white" {}
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

            sampler2D _EmTex;
            float4 _EmTex_ST;

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
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 emCol = tex2D(_EmTex, i.uv);
                emCol = smoothstep(0.6,1,emCol);
                emCol.xyz *= _Color.xyz * _ArrowStr;
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                //fixed4 col = fixed4(i.uv.xy,0,1);
                clip(col.x - 0.5f);
                col.xyz *= emCol.xyz;
                //col.xyz *= abs(sin(_Time.y*2));

                //col.w = _ArrowAlpha;
                //col.xyz *= _Color.xyz;
                return float4(col.xyz,_Color.w);
                
            }
            ENDCG
        }
    }
}

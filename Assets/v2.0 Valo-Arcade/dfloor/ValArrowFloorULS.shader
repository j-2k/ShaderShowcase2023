Shader "Unlit/ValArrowFloorULS"
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
                fixed4 col = tex2D(_MainTex, i.uv);
                //col.xyz += length(i.uv);

                //clip(col.xyz - 0.01f);
                col.xyz = (lerp(0,1,col) * _ArrowStr) * float3(1,0,1);
                
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float3 l = (1 - (length(i.uv - 0.5) * 10));// * float3(1,0,1);
                float3 l2 = (saturate(step(0,l) - step(0.2,l) + l));// * float3(1,0,1);       
                //clip(l-0.3);

                //clip(l-0.3);
                col.xyz += l2;
                clip(col.x - 0.01);



                //float4 fc = float4(l2,1);
                //return fc;
                return float4(col.xyz,_Color.w);
            }
            ENDCG
        }
    }
}

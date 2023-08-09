Shader "Unlit/ValDanceFloorULS"
{
    Properties
    {
        _MainTex ("Hexagon Texture", 2D) = "white" {}
        _CheckerTex ("Checker Texture", 2D) = "white" {}
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
            
            sampler2D _CheckerTex;
            float4 _CheckerTex_ST;

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

            float3 palette( in float t, in float3 a, in float3 b, in float3 c, in float3 d )
            {
                return a + b*cos( 6.28318*(c*t+d) );
            }            

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 sqTex = tex2D(_CheckerTex, i.uv + _Time.y * 0.05);
                clip(col.xyz - 0.5f);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                //float3 col1 = (palette(_Time.y*0.3,float3(0.5,0.5,0.5),float3(0.5,0.5,0.5),float3(1.0,1.0,1.0),float3(0.5,0.2,0.90)).rgb);
                //float3 col2 = (palette(_Time.y*0.3,float3(0.1,0.5,0.9),float3(0.8,0.5,0.6),float3(2.0,1.0,2.0),float3(0.9,0.2,0.60)).rgb);
                
                //col.xyz *=  col1 * sqTex + col2 * (1 - sqTex);
                //col.xyz *= sqTex;

                float3 xyz = lerp(float3(0,0,0), float3(0.5,0.5,0.5), sin(sqTex + _Time.y)*0.5 + 0.5);

                return float4((xyz),_Color.w);
            }
            ENDCG
        }
    }
}

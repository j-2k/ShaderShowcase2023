Shader "Unlit/DDRArrowFloor"
{
    Properties
    {
        _MainTex ("Hexagon Texture", 2D) = "white" {}
        _Alpha ("ALPHA CONTROL", Range(0,1)) = 0.7
        _Size("ArrowSize", Range(1,2)) = 1
        _ArrowStr("Arrow Strength", Range(0,2)) = 0
        _Bloom("Rim Bloom Strength", Range(1,10)) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha  OneMinusSrcAlpha 
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

            float _Alpha;

            float _ArrowStr;
            float _Size;
            float _Bloom;

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

                float2 uv = i.uv * _Size - ((_Size - 1)*0.5);//* sin(_Time.y);
                //float2 uv = i.uv * (sin(_Time.y * 5) * 0.25 + 1.25) - (sin(_Time.y*5) * 0.125 + 0.125);
                fixed4 col = tex2D(_MainTex, uv);
                //clip(col.z - 0.1);
                clip(col.a - 0.1);

                //fixed3 iCol = (1 - col.x);
                //col = saturate(smoothstep(0,1,col) + 2);
                //col = smoothstep(0,1,col.x); //black white arrow
                //float l = (length(i.uv - 0.5) + 1 * sin(_Time.y * 5 + 2) * 0.5 + 0.5);
                //col = 1 - lerp(0,1,col.x);
                
                //sin(_Time.y * 2.5) * 0.5 + 0.5);
                float3 nc = col.b > 0.05 ? 0 : 1;

                //nc = 1-nc;
                //nc.r = 0;
                
                
                //float4 fc = float4(l2,1);
                //return fc;
                return float4(nc*(_Bloom) + pow(col.xyz,3) + _ArrowStr,_Alpha);
                //return float4(l.xxx + col.xyz,_Color.w);
            }
            ENDCG
        }
    }
}

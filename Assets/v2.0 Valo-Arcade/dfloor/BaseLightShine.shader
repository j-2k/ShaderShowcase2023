Shader "Unlit/BaseLightShine"
{
    Properties
    {
        _Color ("Main Color", color) = (0.83,0,1,0.5)
        _Alpha ("_Alpha", Range(0,1)) = 1
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
            float3 _Color;
            float _Alpha;

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
                //fixed4 col = tex2D(_MainTex, i.uv);
                float2 uv = i.uv + 2.8;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                float l = smoothstep(1.9,3,uv.y);
                //l += 1;
                float sl =  smoothstep(2.9,3,uv.y);
                l -= sl;
                //l = max(l,0);
                //l *= -1;
                _Alpha = 1 - _Alpha;

                return float4(_Color.xyz,max(l - _Alpha,0));
            }
            ENDCG
        }
    }
}

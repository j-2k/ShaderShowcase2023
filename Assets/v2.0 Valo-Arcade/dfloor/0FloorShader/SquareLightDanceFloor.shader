Shader "Unlit/SquareLightDanceFloor"
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
                float4 tCol : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv*1.25);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                float2 uv = i.uv;
                float circles = saturate(lerp(0,1,length(frac(uv*5)*2-1)));
                //return float4(circles.xxxx);
                col.xyz += circles;
                //boxs+= frac(uv);
                //float2 circles = frac(smoothstep(0,1,length(uv*2)));
                return float4((col.xyz),1);
            }
            ENDCG
        }
    }
}

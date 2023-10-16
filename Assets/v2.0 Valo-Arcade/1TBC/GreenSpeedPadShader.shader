Shader "Unlit/GreenSpeedPadShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", color) = (0.1,1,0.1,1)
        [HDR] _Emisson ("Emisson", color) = (0.1,0.1,0.1,0.1)
        _Var1 ("Weird Shape", float) = 1
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

            float4 _Color;
            float4 _Emisson;
            float _Var1;

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
                fixed col = tex2D(_MainTex, i.uv ).r ;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                float uvx = i.uv.x * 10;
                /*
                float l = lerp(0,1,1 - sin(uv.x));
                float4 shape = abs(sin(uv.x*3.14/2));
                float4 fs = step(shape,0.99);
                */

                float fs = asin(abs(sin(uvx *3.14 * 0.1) * _Var1));
                
                float steps = smoothstep(0.2,sin(i.uv.y * 15 + fs * 4 + _Time.y * 3),0.1);

                float mask = col * steps + 1 - col;

                clip(mask - 0.01);
                float3 fc = mask * _Color.xyz * _Emisson.xyz;

                UNITY_APPLY_FOG(i.fogCoord, fc);
                
                return float4(fc,1);
            }
            ENDCG
        }
    }
}

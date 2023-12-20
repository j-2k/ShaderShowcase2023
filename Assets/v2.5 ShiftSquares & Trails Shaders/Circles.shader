Shader "Unlit/Circles"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offset("Offset", Range(0,6.28)) = 0
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}// "Queue"="Transparent"}
        //Blend SrcAlpha OneMinusSrcAlpha
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
            float _Offset;
            float4 _Color;

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
                fixed4 col = tex2D(_MainTex, float2(i.uv.x*1, i.uv.y*0.25+0.75));
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);

                //Sphere stuff
                float2 uv = float2(frac(i.uv.x * 4), i.uv.y)*2-1;
                //uv.x += frac(sin(_Time.y));
                //- (sin(_Time.y)*0.2+0.2) | (sin(_Time.y*2)*0.25+0.25)
                float offsetX = (1 - i.uv.x * 0.75);
                float d = 1-step(1 * offsetX, length(uv));
                clip(d-0.1);
                
                //Rectangle
                /*
                float2 uv = (i.uv);
                float r = 1-saturate(sin(uv.y * 3.14*1.25+_Offset));
                clip(r);
                return float4(_Color.xyz * r + ((1 - r) * float3(1,0,0)),1);
                */

                
                return lerp(col,1 - col,sin(_Time.y*4)*0.5+0.5);
                //saturate(col + d)
                //return (1-i.uv.x);
                
                //return float4(i.uv,0,1);
            }
            ENDCG
        }
    }
}

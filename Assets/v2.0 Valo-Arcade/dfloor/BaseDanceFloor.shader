Shader "Unlit/BaseDanceFloor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Col", color) = (1,1,1,1)
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

            float3 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float2 tile(float2 _st, float _zoom){
                _st *= _zoom;
                return frac(_st);
            }

            float2 box(float2 _st, float2 _size, float _smoothEdges){
                _size = 0.5 - _size * 0.5;
                float2 aa = float2(_smoothEdges*0.5,_smoothEdges*0.5);
                float2 uv = smoothstep(_size,_size+aa,_st);
                uv *= smoothstep(_size,_size+aa,float2(1.0,1.0)-_st);
                return uv.x*uv.y;
            }

            float3 createBox(float2 _st, float2 _size, float _smoothEdges, float3 color)
            {
                //return float3(box(uv, sin(_Time.y * 3.141592) * .05 + 0.8, 0.05).xyx) * col * _Color * 2;
                return float3(box(_st, _size,_smoothEdges).xyx * color);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv * 2.5);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                float2 uv = tile(i.uv,10);
                //float3 finalBox = float3(box(uv, abs(sin(_Time.y * 3.141592)) * .25 + 0.75, 0.05).xyx) * col * _Color * 2;
                /*float3 lerpedBox = lerp(createBox(uv, sin(_Time.y * 3.141592) * .05 + 0.8, 0.05, col),
                                        createBox(uv, sin(_Time.y * 3.141592) * .05 + 0.8, 0.05, 1 - col), 
                                        cos(_Time.y * 2)*2 + 1);*/

                //scrolling black bar
                float scrollBlack = lerp(0.9,1,abs(sin(i.uv.y*3.14 + _Time.y*3.141592)*1));
                //float3 finalBox = float3(box(uv, abs(sin(_Time.y * 3.141592)) * .25 + 0.5 * scrollBlack, 0.05).xyx) * col * 2;
                float3 finalBox = float3(box(uv, (sin(_Time.y * 3.141592)) * .1 + 0.75 * scrollBlack, 0.05).xyx) * col * 2;

                //float3 finalCols = finalBox * scrollBlack;

                //return float4(scrollBlack.xxx,1);
                //return float4(finalBox,1);
                //finalBox = lerp(1,0,finalBox).xyz;
                return float4(finalBox * _Color,1);
            }
            ENDCG
        }
    }
}

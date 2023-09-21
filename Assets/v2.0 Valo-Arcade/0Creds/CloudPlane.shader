Shader "Unlit/CloudPlane"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColB("Color B", color) = (0,0,0,0)
        _ColW("Color W", color) = (1,1,1,1)
        _ss0("smoothstep 0", float) = 0
        _ss1("smoothstep 1", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend One OneMinusSrcAlpha
        Cull Off
        ZTest LEqual
        ZWrite On

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 vertCols : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 _ColB;
            float3 _ColW;

            float _ss0;
            float _ss1;

            v2f vert (appdata v)
            {
                v2f o;
                o.normal = v.normal;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);


                float staticSample = tex2Dlod(_MainTex,float4(o.uv,0,0)).r;
                o.uv.x += _Time.y*0.1;
                float movingSample = tex2Dlod(_MainTex,float4(o.uv,0,0)).r;

                float finSample = (movingSample + staticSample)*0.5;
                float remap = abs(finSample * 2 - 1); //(if 1 => 1) (if 0 => -1) 1 to -1 range
                float ssRemap = smoothstep(_ss0,_ss1,remap);
                o.vertCols = float3(ssRemap.xxx);         
                v.vertex.y += o.normal.y * ssRemap * 20;

       


                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                float3 fc = lerp(_ColB,_ColW,i.vertCols.x);
                return float4(fc,0.5);
                //return float4(i.vertCols.xxx,1);
            }
            ENDCG
        }
    }
}

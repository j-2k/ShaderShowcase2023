// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Eclipse97"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Bgc("Background Color", color) = (0.39,0,0,1)
        _Ec("Eclipse Color", color) = (0,0,0,1)
        _Ec2("Eclipse Ring Color 2", color) = (1,0.5,0.5,1)
        _Ic("Iris Color", color) = (0,0.5,0.5,1)
        _Ic2("Iris Ring Color 2", color) = (0,0.5,0.5,1)

        _EyeRange("Eye Closing Control", Range(1,20)) = 1 

        _p1("Property1", float) = 0.5
        _p2("Property2", vector) = (0.5,0.5,0.5,1)
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

            #define PIE 3.14159

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 sp : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float3 vsn : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Bgc;
            float _p1;
            float4 _Ec;
            float4 _Ec2;
            float4 _Ic;
            float4 _Ic2;
            vector _p2;
            float _EyeRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                float3 ws = mul(unity_ObjectToWorld, v.vertex);
                //o.normal = UnityObjectToWorldNormal(v.normal);
                //o.sp = ComputeScreenPos(float4(o.normal,1)).xyz;
                //o.vsn = COMPUTE_VIEW_NORMAL;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                float4 euv = float4(i.uv.xy,0,1);

                float2 cluv = i.uv.xy;
                
                //https://www.desmos.com/calculator/a6xgyqctvx
                //float sw = abs((sin(_Time.y)) * 9) + 1;

                cluv.y = (cluv.y * _EyeRange) - _EyeRange/2 + .5; //max blink eye
                float2 cl = length(cluv * 2 - 1);


                float2 cuv = i.uv.xy;
                cuv.x = (cuv.x * 6) - 2.5;


                cuv.y = (cuv.y * (0.5 + _EyeRange)) - _EyeRange/2 + .25;//(sw/2 - 0.5); //iris y fix

                //cuv.y = (cuv.y * 20) - 9.5; //max blink iris
                float li = length(cuv * 2 - 1);

                float luv = 1 - smoothstep(0.3,0.35,cl);
                float luv2 = 1 - smoothstep(0.35,_p1,cl);
                float luvi = 1 - smoothstep(0.3,0.35,li);
                float luvi2 = 1 - smoothstep(0.35,_p1,li);

                float4 fc = (luv - luvi2 ) * _Ec;
                float4 fc2 = (luv2 - luv) * _Ec2;
                float4 iris = (luvi) * _Ic;
                float4 iris2 = (luvi2 - luvi) * _Ic2;



                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv.xy);


                float4 fbgc = (1 - luv2)  * _Bgc;
                float4 eye = fbgc + ((fc + fc2 + iris + iris2) * 1);
                return eye;
                //return float4(i.vsn.xy,0,1); //(1 - luv);
                /*
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
                */
            }
            ENDCG
        }
    }
}

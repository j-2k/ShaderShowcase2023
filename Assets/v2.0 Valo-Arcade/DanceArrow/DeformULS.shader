// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
//https://youtu.be/MPemOdTvHTk?t=108
Shader "Unlit/DeformULS"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", color) = (1,1,1,1)
        [HDR] _Emission ("Fresnel Emission Color", color) = (1,1,1,1)
        _FresnelExponent ("Fresnel Exponent", float ) = 0.2
        _TimeScale ("Time Scale", float) = 0.5
        _TimeOffset ("Time Offset", float) = 0.5
        _Amount("Wave Amount", float) = 0.2
        _Strength("_Strength", Range(0,2)) = 0.2
        _UpPow("Up power", Range(-1,0)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Geometry"}
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD3;
                float3 wNormal : TEXCOORD2;
                float3 viewDir : POSITION1;
                float3 worldPos : POSITION2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Emission;
            float4 _Color;
            float _Amount;
            float _TimeScale;
            float _TimeOffset;
            float _Strength;

            float _FresnelExponent;

            float _UpPow;

            

            v2f vert (appdata v)
            {
                v2f o;
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
                o.normal = v.normal;
                o.wNormal = normalize(v.normal);

                //v.vertex.x += (sin(v.vertex.y * _Amount + (_Time.y * _TimeScale)) * 0.03);
                float upwardLerp = lerp(0.1,1.2,(v.vertex.y+1)/2);
                //all giga under here
                //if(v.vertex.y >= -0.2)
                {
                    v.vertex.xz += sin(((v.vertex.xz+1)/2) * _Amount + (_Time.y * _TimeScale + _TimeOffset * 2)) * 0.1  * (pow(upwardLerp,1) * _Strength);
                    v.vertex.y += cos(((v.vertex.y+1)/2) * _Amount + (_Time.y * _TimeScale + _TimeOffset * 2))* 0.1 * (pow(upwardLerp,1) * _Strength);
                }
                //v.vertex.y += (cos(v.vertex.x * _Amount + (_Time.y * _TimeScale)) * 0.03);


                {
                    float lerpUp = lerp(0.5,2,v.vertex.y + 0.5);
                    v.vertex.y -= lerpUp * _UpPow;
                }

                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex.y += abs(sin(_Time.y * 0));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float f = dot(i.wNormal,i.viewDir);
                f = saturate(1 - f);
                f = pow(f, _FresnelExponent);

                float3 fc = (_Emission.xyz * f);
                return float4(fc,1) + _Color;
            }
            ENDCG
        }
    }
}

Shader "Unlit/SphericalXY"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Row("Row r (r, Radius)", Range(-1,2)) = 1
        _Theta("Theta  θ (x, X axis angle 0 to 2PI)", Range(0,6.283)) = 0
        _Phi("Phi φ (z but y in unity, Y axis angle 0 to PI)", Range(0,3.141)) = 0
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Row;
            float _Theta;
            float _Phi;

            #define PI 3.141
            #define TAU 6.283

            v2f vert (appdata v)
            {
                v2f o;
                o.normal = (v.normal);
                v.vertex.xyz += (o.normal * (_Row*0.5));
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                //SOLVE FOR NEW UV Y WHICH IS PHI
                float spherePHI = acos(i.uv.y);
                float sphereTHETA = atan2(i.uv.y,i.uv.x);
                //return sphereTHETA;

                //Showcase Spherical Coordinates;
                float2 sphericalUV = float2(sphereTHETA,spherePHI);
                
                //UV Y = GREEN IS PHI
                //UV X = RED IS THETA


                
                fixed4 col = tex2D(_MainTex, i.uv);
                
                return col;
                //return float4(sphericalUV,0,1);
                //return float4(i.normal ,1);
            }
            ENDCG
        }
    }
}

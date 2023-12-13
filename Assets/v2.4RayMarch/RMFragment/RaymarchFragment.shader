Shader "Unlit/RaymarchFragment"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SpherePos("Sphere Position", Vector) = (0,1,6,1)
        _LightPos("Light Position", Vector) = (0,1,5,1)
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
                float3 camPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _SpherePos;
            float4 _LightPos;

            v2f vert (appdata v)
            {
                v2f o;
                o.camPos = _WorldSpaceCameraPos;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            #define MAX_DIST 100.0
            #define MIN_SURF_DIST 0.001
            #define MAX_STEPS 100

            float GetDistance(float3 distancePoint)
            {
                float dSphere = length(distancePoint - _SpherePos.xyz) - _SpherePos.w;
                float dPlane = distancePoint.y;
                float dRaymarch = min(dSphere, dPlane);
                return dRaymarch;
            }

            float3 GetNormals(float3 p)
            {
                float d = GetDistance(p);
                float2 e = float2(0.01, 0);

                float3 normals = d - float3(
                    GetDistance(p - e.xyy),
                    GetDistance(p - e.yxy),
                    GetDistance(p - e.yyx)
                );
                return normalize(normals);
            }

            float GetLight(float3 p)
            {
                //_LightPos.xz += float2(sin(_Time.y),0) * 5;
                float3 lightDir = normalize(_LightPos - p);
                float3 normal = GetNormals(p);

                float dotNL = saturate(dot(normal, lightDir));

                return dotNL;
            }



            //github copilot instantly put the raymarch code inside, kinda cool but not exactly what I wanted but kinda close, so i just refactored.
            float RayMarch (float3 rayOrigin, float3 rayDirection)
            {
                float dO = 0.0; //Distance from Origin
                float dS = 0.0; //Distance from Scene
                for (uint i = 0; i < MAX_STEPS; i++)
                {
                    float3 p = rayOrigin + rayDirection * dO;             // standard point calculation dO is the offset for direction or magnitude
                    dS = GetDistance(p);                             
                    dO += dS;
                    if (dS < MIN_SURF_DIST || dO > MAX_DIST) break;            // if we are close enough to the surface or too far away, break
                }
                return dO;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 cuv = i.uv * 2 - 1;

                float3 rayOrigin = float3(0,1,0);
                float3 rayDirection = normalize(float3(cuv.xy,1));

                float distanceRM = RayMarch(rayOrigin, rayDirection);//i.camPos

                float3 p = rayOrigin + rayDirection * distanceRM;
                
                float3 light = GetLight(p);
                
                //float3 diff = GetNormals(p); test normals
                
                //distanceRM /= _SpherePos.z;
                
                return float4(light.xyz,1);

                /*
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
                */
            }
            ENDCG
        }
    }
}

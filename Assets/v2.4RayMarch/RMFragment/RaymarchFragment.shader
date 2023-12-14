Shader "Unlit/RaymarchFragment"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _CameraOrigin("Camera Position", Vector) = (0,1,0,1)
        _PlanePos("Plane Position", Vector) = (0,0,0,1)
        _SpherePos("Sphere Position", Vector) = (0,1,8,1)
        _LightPos("Light Position", Vector) = (0,1,8,4) //w is rotation magnitude offset
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

            //sampler2D _MainTex;
            //float4 _MainTex_ST;

            float4 _SpherePos;
            float4 _LightPos;
            float4 _CameraOrigin;
            float4 _PlanePos;

            v2f vert (appdata v)
            {
                v2f o;
                o.camPos = _WorldSpaceCameraPos;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            //NOTES
            //NOTE 1:
            //float dPlane = distancePoint.y didnt make sense at first, so I manually got the direction or distance by manually subtracting the y values.
            //I was thinking that the distance point was static for some reason even though ik it isnt (thinking it was camera origin only)??? but it is not, it is the point that is being raymarched, so it is constantly changing. obviously.
            //Now I think of the distancePoint as a camera origin that is constantly moving forward if that makes sense, even though thats ... wrong to think? idk,
            //but that helped me understand it abit more, so now I know getting the plane is just the y axis of the distance point since when raymarching
            //towards the sphere it will yield a y value that constantly gets lower and lower until it hits the sphere(assuming the sphere is placed on the floor of the plane). 
            //sorry that this is long over something that is so easy to understand but I just wanted to write it down so I can remember it.

            //NOTE 2:


            #define MAX_DIST 100.0
            #define MIN_SURF_DIST 0.001
            #define MAX_STEPS 100


            //IMPORTANT: GET DISTANCE IS USED TO GET THE DISTANCE OF EVERYTHING IN THE SCENE, SO IF YOU WANT TO ADD MORE OBJECTS, YOU NEED TO ADD IT HERE.
            //THIS IS KEY WHEN UNDERSTANDING HOW RAYMARCHING ACTUALLY WORKS. The raymarching algortihim is so simple tbh but understanding distances is 10x more important.
            float GetDistance(float3 distancePoint)
            {
                float dSphere = length(distancePoint - _SpherePos.xyz) - _SpherePos.w;
                float dPlane = distancePoint.y - _PlanePos.y;// REFERENCE NOTE 1 // for some reason i had a hard time understanding just (dPlane = distancePoint.y).
                float distanceToScene = min(dSphere, dPlane);         //get min from the 2 objects so we dont step into something we dont want to.
                return distanceToScene; //distance to scene is the distance scalar from ANYTHING in the scene
            }


            //in order to get normals on complex objects that have a curve on them its fairly simple, you have to sample 2 points inifi close to each other and 
            //draw a line between them, effectively the slope & then you get the normal from that line! pretty crazy, cant believe im using the slope formula when back then I use to say when tf am i going to use this.
            float3 GetNormals(float3 p)
            {
                float d = GetDistance(p);
                float2 e = float2(0.01, 0);

                float3 normals = d - float3(
                    GetDistance(p - e.xyy),
                    GetDistance(p - e.yxy),
                    GetDistance(p - e.yyx)
                );
                    /*trying to understand how partial derivatives work, slightly missing how this gives you a correct normal vector
                    same as above but with partial derivatives
                    float df_dx = (d - GetDistance(p - e.xyy));
                    float df_dy = (d - GetDistance(p - e.yxy));
                    float df_dz = (d - GetDistance(p - e.yyx));
                    return normalize(float3(df_dx, df_dy, df_dz));
                    */

                    //ok now i kind of understand, after tons of images and desmos trials but a simple summary is to compare the distances of the shifted points (shifting the points means the whole sphere will move with it!) 
                    //to the original points in the 4 quadrants. (the result from [original distance point] - [shifted distance point] is you get a x and y value that is the vector/correct color gradient to be used as the normal! ) 
                    //here is a extremely bad drawing of what i was doing and figured it out? https://prnt.sc/DQRrOrAIYs1c i might still be wrong but the idea at least is in my head now. will revisit this later.
                    //thats basically how to get normals the idea is commented above this function for a reminder.
                return normalize(normals);
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
                    if (dS < MIN_SURF_DIST || dO > MAX_DIST) break;            // if we are close enough to a surface or went to infinity, break & return distance to the origin
                }
                return dO;
            }

            float GetLight(float3 p)
            {
                _LightPos.xz += float2(sin(_Time.y*2),cos(_Time.y*2))*_LightPos.w;
                float3 lightDir = normalize(_LightPos - p);
                float3 normal = GetNormals(p);

                float dotNL = saturate(dot(normal, lightDir));
                float d = RayMarch(p + normal * (MIN_SURF_DIST * 2), lightDir);
                if (d < length(lightDir)) 
                {
                    dotNL *= smoothstep(0.7, 1, d / length(lightDir));
                    //dotNL *= 0.1;
                }

                return dotNL;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 cuv = i.uv * 2 - 1;

                float3 rayOrigin = _CameraOrigin;
                float3 rayDirection = normalize(float3(cuv.xy,1));

                float distanceRM = RayMarch(rayOrigin, rayDirection);//i.camPos
                if(distanceRM > MAX_DIST) return float4(0,0.4,0.8,1);

                float3 p = rayOrigin + rayDirection * distanceRM;
                
                float3 light = GetLight(p);
                
                float3 diff = GetNormals(p); //test normals
                return float4(diff,1);
                //distanceRM /= _SpherePos.z;
                
                return float4(light.xyz * float3(1,1,0),1);

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

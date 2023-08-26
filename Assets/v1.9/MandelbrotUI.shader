Shader "Hidden/MandelbrotUI"
{
    Properties
    {
        _MainTex ("Gradient Texture", 2D) = "white" {}
        _Area("Area Center", vector) = (0,0,4,4)
        _Angle("Angle", Range(-3.14,3.14)) = 0
        _MaxIteration("Max Iteration", Range(0,500)) = 0
        _Color("Color", Range(0,1)) = 0.5
        _Repeat("Repeat", float) = 1
        _Speed("Time Speed", float) = 0.5
    }
    SubShader
    {
        // No culling or depth
        Cull Off
        ZWrite On //should be off, but since I have it on a scene need depth buffer
        //ZTest Always

        /* default for ui image
        Cull Off
        ZWrite Off
        ZTest Always
        */

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 _Area;
            sampler2D _MainTex;
            float _Angle;
            float _MaxIteration;
            float _Color;
            float _Repeat;
            float _Speed;

            float2 rotate(float2 p, float2 pivot, float angle)
            {
                float rSin = sin(angle);
                float rCos = cos(angle);

                p -= pivot;
                p = float2( p.x * rCos - p.y * rSin, p.x * rSin + p.y * rCos);
                p += pivot;


                return p;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                /*
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                */


                float2 uv = i.uv - 0.5f;

                //symmetry XY
                //uv = abs(uv);

                float2 c = _Area.xy + uv * _Area.zw;
                c = rotate(c,_Area.xy,_Angle);

                //interpolation
                float r = 20; //max of radius
                float r2 = r * r; 

                float2 zPrev;
                float2 z;
                float iteration;
                for (iteration = 0; iteration < _MaxIteration; iteration++)
                {
                    zPrev = rotate(z,0,_Time.y);
                    z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + c;
                    
                    
                    if(dot(z,zPrev) > r2)
                    {
                        break;
                    }
                    
                }

                if(iteration > _MaxIteration)
                {
                    return 0;
                }

                float dist = length(z); //dist from origin
                //float fracIter = (dist - r) / (r2 - r); //range 0 - 1 & is a linear interpolation
                float fracIter = log2(log(dist) / log(r)); //double exponential interpolation


                //iteration -= fracIter;

                float finalMandelbrot = sqrt(iteration/_MaxIteration);
                //float4 finalCol = sin(float4(.35,.9,.5,1) * finalMandelbrot * 20) * .5 + .5; 
                float4 finalCol = tex2D(_MainTex,float2(finalMandelbrot * _Repeat + _Time.y*_Speed,_Color));//final mandelbrot left to right, col up to down

                float angleOrigin = atan2(z.x,z.y);

                finalCol *= smoothstep(3.5,0,fracIter);

                finalCol *= 1 + sin(angleOrigin * 3 + _Time.y * 6) * .2;

                return saturate(finalCol);

                //return col;
            }
            ENDCG
        }
    }
}

Shader "Unlit/SDFBasic"
{
    Properties
    {
        _Color ("Color", color) = (1,1,1,1)
        _IC ("I Color", color) = (1,1,1,1)
        _OC ("O Color", color) = (1,1,1,1)
        _LineDistance ("_LineDistance", float) = 1
        _LineThickness ("_LineThickness", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        //Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "2D_SDF.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 worldPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float4 _IC;
            float4 _OC;
            float _LineDistance;
            float _LineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld,v.vertex);
                return o;
            }

       

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = sceneRect(i.worldPos.xz);
                //float distChange = fwidth(dist) * 0.5;
                //float AAcutoff = smoothstep(distChange,-distChange,dist);
                fixed4 col = lerp(_IC, _OC, step(0, dist));

                float distanceChange = fwidth(dist) * 0.5;
                float majorLineDistance = abs(frac(dist / _LineDistance + 0.5) - 0.5) * _LineDistance;
                float majorLines = smoothstep(_LineThickness - distanceChange, _LineThickness + distanceChange, majorLineDistance);
                return col * majorLines;

                //return col;
            }
            ENDCG
        }
    }
    FallBack "Standard"
}

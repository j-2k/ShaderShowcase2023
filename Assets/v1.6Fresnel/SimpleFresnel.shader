Shader "Custom/SimpleFresnel"
{
    Properties
    {
        //_Glossiness ("Smoothness", Range(0,1)) = 0.5
        //_Metallic ("Metallic", Range(0,1)) = 0.0
        [HDR] _Emission ("Emission", color) = (0,0,0)

        _FC ("Fresnel Color", color) = (1,1,1,1)
        [PowerSlider(4)] _FE ("Fresnel Exponent", Range(0.1,4)) = 1

        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha 
        Cull Back
        LOD 200
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert alpha fullforwardshadows vertex:vert 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct appdata_particles {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float4 color : COLOR;
        float4 texcoords : TEXCOORD0;
        };

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 viewDir;
            float Custom1;
            INTERNAL_DATA
        };

        void vert(inout appdata_particles v, out Input o) {
        UNITY_INITIALIZE_OUTPUT(Input,o);
        o.Custom1 = v.texcoords.z;
        }

        //half _Glossiness;
        //half _Metallic;

        float3 _Emission;

        float4 _FC;
        float _FE;

        float _Cutoff;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;

            float f = dot(IN.worldNormal,IN.viewDir);
            f = saturate(1 - f);
            f = pow(f, _FE);
            float4 fc = f * _FC;

            o.Emission = _Emission + fc;
            o.Albedo = fc.rgb;////IN.Custom1;
            o.Alpha = saturate(fc.a - _Cutoff - IN.Custom1);//saturate(f-_Cutoff);
        }
        ENDCG
    }
    FallBack "Diffuse"
}

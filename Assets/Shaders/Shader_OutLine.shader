Shader "Custom/Shader_OutLine"
{
    Properties
    {
        _OutLineColor ("Color", Color) = (1, 1, 1, 1)
        _OutLineWidth ("Width", Float) = 1
    }
    SubShader
    {
        
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" "RenderType" = "Transparent" }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        CBUFFER_START(UnityPerMaterial)
        float4 _OutLineColor;
        float _OutLineWidth;
        CBUFFER_END
        ENDHLSL
        
        Cull Front
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            Name "URPSimpleLit"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            #pragma vertex vert
            #pragma fragment frag
            
            struct Attributes
            {
                float4 positionOS: POSITION;
                float4 normalOS: NORMAL;
            };
            struct Varings
            {
                float4 positionCS: SV_POSITION;
            };
            
            Varings vert(Attributes IN)
            {
                Varings OUT;
                IN.positionOS += normalize( IN.normalOS) * _OutLineWidth;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = positionInputs.positionCS;            
                return OUT;
            }
            
            float4 frag(Varings IN): SV_Target
            {
                return _OutLineColor;
            }
            ENDHLSL
            
        }
    }
}


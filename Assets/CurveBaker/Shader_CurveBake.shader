Shader "Custom/Curve"
{
    Properties
    {
      
        _CurveFactor ("CurveFactor", Float) = 1 
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Geometry"
            "RenderType"="Opaque"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        CBUFFER_START(UnityPerMaterial)
        float _CurveFactor;
        CBUFFER_END
        
        ENDHLSL
    

        Pass
        {
            Name "URPSimpleLit" 
            Tags{"LightMode"="UniversalForward"}

            HLSLPROGRAM            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };
            struct Varings
            {
                float4 positionUV : SV_POSITION;
                //float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float3 normalWS : TEXCOORD3;
            };
               

            Varings vert(Attributes IN)
            {
                Varings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS.xyz);
                //OUT.positionCS = positionInputs.positionCS;
                OUT.positionWS = positionInputs.positionWS;
                OUT.viewDirWS = GetCameraPositionWS() - positionInputs.positionWS;
                OUT.normalWS = normalInputs.normalWS;       
                
                //烘焙用
                float4 uvPos = float4(IN.uv.xy, 0.0, 1.0);
                OUT.positionUV = mul(UNITY_MATRIX_P, uvPos);
                return OUT;
            }
            
            float4 frag(Varings IN):SV_Target
            {
                
                float worldBump=normalize(IN.normalWS);
                float worldPos=IN.positionWS;
                float cuv = length(fwidth(worldBump)) / length(fwidth(worldPos)) / 10000 * _CurveFactor;
                return float4(cuv,cuv,cuv,1);
            }
            ENDHLSL            
        }

    }
}

Shader "Custom/SetUpFace"
{
    Properties
    {
        [MainTexture][NoScaleOffset]_BaseMap ("Base Texture", 2D) = "white" { }
        [HideInInspector]_BaseColor ("BaseColor", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_FacialPattenMask ("Facial Patten Mask", 2D) = "white" { }
        
        [NoScaleOffset]_PaintBrushTexture ("Paint Brush Texture", 2D) = "white" { }
        _PaintBrushIntensity ("PaintBrushIntensity", Range(0.0, 1.0)) = 0.75   
        
        [NoScaleOffset] _SpecularMask ("Specular Mask", 2D) = "white" { }
        _SpecularColor ("SpecularColor", Color) = (1, 1, 1, 1)
        _SpcularGloss ("SpcularGloss", float) = 10
        
        [NoScaleOffset][Normal] _BumpMap ("Normal Map", 2D) = "bump" { }
        _BumpScale ("BumpScale", Range(-1.0, 1.0)) = 1
        
        [NoScaleOffset] _CurveMap ("Curve Map", 2D) = "black" { }
        [NoScaleOffset] _SSSRampMap ("SSS Ramp Map", 2D) = "white" { }
        _SSSScale ("SSS Scale", Range(0.0, 10.0)) = 1     
        
        
        _Cutoff ("Cutoff", Range(0.0, 1.0)) = 0.5
        
        _FacialPattenIntensity ("FacialPattenIntensity", Range(0.0, 1.0)) = 0.75        
        [NoScaleOffset]_FacePartTexture ("Face Part Texture", 2D) = "black" { }
        [NoScaleOffset]_FacePartTexture2 ("Face Part2 Texture", 2D) = "black" { }
        _FacePartColor ("Face Part Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_ForeHeadPartTexture ("ForeHead Part Texture", 2D) = "black" { }
        _ForeHeadPartColor ("ForeHead Part Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_BrowPartTexture ("Brow Part Texture", 2D) = "black" { }
        _BrowPartColor ("Brow Part Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_EyePartTexture ("Eye Part Texture", 2D) = "black" { }
        _EyePartColor ("Eye Part Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_NosePartTexture ("Nose Part Texture", 2D) = "black" { }
        _NosePartColor ("Nose Part Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_MousePartTexture ("Mouse Part Texture", 2D) = "black" { }
        _MousePartColor ("Mouse Part Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_DecoPartTexture ("Deco Part Texture", 2D) = "black" { }
        _DecoPartColor ("Deco Part Color", Color) = (1, 1, 1, 1)
        
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" "RenderType" = "Opaque" }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        
        CBUFFER_START(UnityPerMaterial)
        float4 _BaseMap_ST;
        float4 _BaseColor;
        float4 _SpecularColor;
        float _SpcularGloss;
        float _Spcular2Scale;
        float _BumpScale;
        float _SSSScale;
        float _Cutoff;
        float _FacialPattenIntensity;
        float _PaintBrushIntensity;
        half4 _FacePartColor;
        half4 _ForeHeadPartColor;
        half4 _BrowPartColor;
        half4 _EyePartColor;
        half4 _NosePartColor;
        half4 _MousePartColor;
        half4 _DecoPartColor;
        CBUFFER_END
        ENDHLSL
        
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
                float2 uv: TEXCOORD0;
                float2 uv2: TEXCOORD1;
            };
            struct Varings
            {
                float4 positionCS: SV_POSITION;
                float4 uv: TEXCOORD0;
                float3 positionWS: TEXCOORD1;
                float3 viewDirWS: TEXCOORD2;
                float3 normalWS: TEXCOORD3;    // xyz: normal, w: viewDir.x
                float3 tangentWS: TEXCOORD4;    // xyz: tangent, w: viewDir.y
                float3 bitangentWS: TEXCOORD5;    // xyz: bitangent, w: viewDir.z
            };
            
            TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
            TEXTURE2D(_PaintBrushTexture);        SAMPLER(sampler_PaintBrushTexture);
            TEXTURE2D(_FacialPattenMask);        SAMPLER(sampler_FacialPattenMask);
            TEXTURE2D(_SpecularMask);   SAMPLER(sampler_SpecularMask);
            TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
            TEXTURE2D(_CurveMap);       SAMPLER(sampler_CurveMap);
            TEXTURE2D(_SSSRampMap);     SAMPLER(sampler_SSSRampMap);
            TEXTURE2D(_FacePartTexture);    SAMPLER(sampler_FacePartTexture);
            TEXTURE2D(_FacePartTexture2);    SAMPLER(sampler_FacePartTexture2);
            TEXTURE2D(_ForeHeadPartTexture);    SAMPLER(sampler_ForeHeadPartTexture);
            TEXTURE2D(_BrowPartTexture);    SAMPLER(sampler_BrowPartTexture);
            TEXTURE2D(_EyePartTexture);    SAMPLER(sampler_EyePartTexture);
            TEXTURE2D(_NosePartTexture);    SAMPLER(sampler_NosePartTexture);
            TEXTURE2D(_MousePartTexture);    SAMPLER(sampler_MousePartTexture);
            TEXTURE2D(_DecoPartTexture);    SAMPLER(sampler_DecoPartTexture);
            
            Varings vert(Attributes IN)
            {
                Varings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS.xyz);
                OUT.positionCS = positionInputs.positionCS;
                OUT.positionWS = positionInputs.positionWS;
                OUT.viewDirWS = GetCameraPositionWS() - positionInputs.positionWS;
                OUT.normalWS = normalInputs.normalWS;
                OUT.tangentWS = normalInputs.tangentWS;
                OUT.bitangentWS = normalInputs.bitangentWS;
                OUT.uv.xy = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.uv.zw = TRANSFORM_TEX(IN.uv2, _BaseMap);
                return OUT;
            }
            
            
            float adjustPaint(float maskValue)
            {
                return smoothstep(0.5f, 1, maskValue * 3);
            }
            
            
            float4 frag(Varings IN): SV_Target
            {
                //Sample Texture
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv.xy);
                half facialPattenMask = SAMPLE_TEXTURE2D(_FacialPattenMask, sampler_FacialPattenMask, IN.uv.zw);
                float4 specularMask = SAMPLE_TEXTURE2D(_SpecularMask, sampler_SpecularMask, IN.uv.xy);
                float curve = SAMPLE_TEXTURE2D(_CurveMap, sampler_CurveMap, IN.uv.xy);
                float3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv.xy), clamp(_BumpScale , -1, 1));
                float3 normalWS = normalize(TransformTangentToWorld(normalTS, half3x3(IN.tangentWS, IN.bitangentWS, IN.normalWS)));
                float paint = SAMPLE_TEXTURE2D(_PaintBrushTexture, sampler_PaintBrushTexture, IN.uv.zw);
                

                half4 faceColor = SAMPLE_TEXTURE2D(_FacePartTexture, sampler_FacePartTexture, IN.uv.zw) * _FacePartColor;
                half4 face2Color = SAMPLE_TEXTURE2D(_FacePartTexture2, sampler_FacePartTexture2, IN.uv.zw);
                half4 foreHeadColor = SAMPLE_TEXTURE2D(_ForeHeadPartTexture, sampler_ForeHeadPartTexture, IN.uv.zw) * _ForeHeadPartColor;
                half4 browColor = SAMPLE_TEXTURE2D(_BrowPartTexture, sampler_BrowPartTexture, IN.uv.zw) * _BrowPartColor;
                half4 eyeColor = SAMPLE_TEXTURE2D(_EyePartTexture, sampler_EyePartTexture, IN.uv.zw) * _EyePartColor;
                half4 noseColor = SAMPLE_TEXTURE2D(_NosePartTexture, sampler_NosePartTexture, IN.uv.zw) * _NosePartColor;
                half4 mouseColor = SAMPLE_TEXTURE2D(_MousePartTexture, sampler_MousePartTexture, IN.uv.zw) * _MousePartColor;
                half4 decoColor = SAMPLE_TEXTURE2D(_DecoPartTexture, sampler_DecoPartTexture, IN.uv.zw) * _DecoPartColor;

                half4 facialPattenColor = baseColor;         
                facialPattenColor.a = smoothstep(0.1f,0.9f,faceColor.a+face2Color.a+browColor.a+eyeColor.a+foreHeadColor.a+noseColor.a+mouseColor.a+decoColor.a)*_FacialPattenIntensity;
                facialPattenColor.rgb = lerp(facialPattenColor.rgb,faceColor.rgb,faceColor.a);
                facialPattenColor.rgb = lerp(facialPattenColor.rgb,face2Color.rgb,face2Color.a);
                facialPattenColor.rgb = lerp(facialPattenColor.rgb,browColor.rgb,browColor.a);
                facialPattenColor.rgb = lerp(facialPattenColor.rgb,eyeColor.rgb,eyeColor.a);
                facialPattenColor.rgb = lerp(facialPattenColor.rgb,foreHeadColor.rgb,foreHeadColor.a);
                facialPattenColor.rgb = lerp(facialPattenColor.rgb,noseColor.rgb,noseColor.a);
                facialPattenColor.rgb = lerp(facialPattenColor.rgb,mouseColor.rgb,mouseColor.a);
                facialPattenColor.rgb = lerp(facialPattenColor.rgb,decoColor.rgb,decoColor.a);

                paint = lerp(float3(1,1,1),paint,_PaintBrushIntensity);
                paint=saturate(paint+(3-(facialPattenColor.r+facialPattenColor.g+faceColor.b))*0.15f);//亮度越低应用纹理越弱
                baseColor.rgb = lerp(baseColor.rgb,facialPattenColor.rgb,facialPattenColor.a * facialPattenMask * paint);
                
                //计算主光
                Light light = GetMainLight();
                half NdotL = dot(normalize(normalWS), light.direction);
                half3 sss = SAMPLE_TEXTURE2D(_SSSRampMap, sampler_SSSRampMap, float2(0.5 * NdotL + 0.5, saturate(curve * _SSSScale)));
                half3 diffuseColor = light.color * sss;
                half3 specularColor = LightingSpecular(light.color, light.direction, normalize(normalWS), normalize(IN.viewDirWS), _SpecularColor, _SpcularGloss);
                //计算附加光照
                uint pixelLightCount = GetAdditionalLightsCount();
                for (uint lightIndex = 0; lightIndex < pixelLightCount; ++lightIndex)
                {
                    Light light = GetAdditionalLight(lightIndex, IN.positionWS);
                    half3 attenuation = light.distanceAttenuation * light.shadowAttenuation;
                    NdotL = dot(normalize(normalWS), light.direction);
                    sss = SAMPLE_TEXTURE2D(_SSSRampMap, sampler_SSSRampMap, float2(0.5 * NdotL + 0.5, saturate(curve * _SSSScale)));
                    diffuseColor += light.color * attenuation * sss;
                    specularColor += LightingSpecular(attenuation * light.color, light.direction, normalize(normalWS), normalize(IN.viewDirWS), _SpecularColor, _SpcularGloss);
                }
                half3 color = baseColor * diffuseColor + specularColor * specularMask;
                
                //color = half3(alpha,alpha,alpha);
                clip(baseColor.a - _Cutoff);
                return float4(color, 1);
            }
            ENDHLSL
            
        }
        
        
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            ZWrite On
            ZTest LEqual
            Cull[_Cull]
            
            HLSLPROGRAM
            
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            
            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA
            
            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            
            
            //由于这段代码中声明了自己的CBUFFER，与我们需要的不一样，所以我们注释掉他
            //#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            //它还引入了下面2个hlsl文件
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
            
        }
    }
}

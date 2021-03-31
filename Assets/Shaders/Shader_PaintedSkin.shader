Shader "Custom/PaintedSkin"
{
    Properties
    {
        [MainTexture][NoScaleOffset]_BaseMap ("Base Texture", 2D) = "white" { }
        [HideInInspector]_BaseColor ("BaseColor", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_FacialPattenTex ("Facial Patten", 2D) = "black" { }
        [NoScaleOffset]_FacialPattenMask ("Facial Patten Mask", 2D) = "white" { }
        _FacialPattenIntensity ("FacialPattenIntensity", Range(0.0, 1.0)) = 0.75
        
        [NoScaleOffset] _SpecularMask ("Specular Mask", 2D) = "white" { }
        _SpecularColor ("SpecularColor", Color) = (1, 1, 1, 1)
        _SpcularGloss ("SpcularGloss", float) = 10
        
        [NoScaleOffset][Normal] _BumpMap ("Normal Map", 2D) = "bump" { }
        _BumpScale ("BumpScale", Range(-1.0, 1.0)) = 1
        
        [NoScaleOffset] _CurveMap ("Curve Map", 2D) = "black" { }
        [NoScaleOffset] _SSSRampMap ("SSS Ramp Map", 2D) = "white" { }
        _SSSScale ("SSS Scale", Range(0.0, 10.0)) = 1
        
        
        _EmissionScale ("Emission Scale", Range(0.0, 1)) = 1
        //_Correction ("Correction Value", Range(0.0, 1.0)) = 0.25
        
        _Cutoff ("Cutoff", Range(0.0, 1.0)) = 0.5
        
        
        
        //[NoScaleOffset]_PaintArea1 ("_PaintArea1", 2D) = "black" { }
        //[NoScaleOffset]_PaintArea2 ("_PaintArea2", 2D) = "black" { }
        
        [HideInInspector][NoScaleOffset]_BumpMask ("Smooth Mask", 2D) = "grey" { }
        [HideInInspector][NoScaleOffset]_ControlMask1 ("_ControlMask1", 2D) = "black" { }
        [HideInInspector][NoScaleOffset]_ControlMask2 ("_ControlMask2", 2D) = "black" { }
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" "RenderType" = "Opaque" }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        
        CBUFFER_START(UnityPerMaterial)
        float4 _BaseMap_ST;
        float4 _BaseColor;
        float _FacialPattenIntensity;
        float4 _SpecularColor;
        float _SpcularGloss;
        float _Spcular2Scale;
        float _BumpScale;
        float _SSSScale;
        float _Cutoff;
        //float _Correction;
        float _EmissionScale;
        CBUFFER_END
        
        uniform float4x4 _MaskColor1;
        uniform float4x4 _MaskColor2;
        
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
            TEXTURE2D(_FacialPattenTex);        SAMPLER(sampler_FacialPattenTex);
            TEXTURE2D(_FacialPattenMask);        SAMPLER(sampler_FacialPattenMask);
            TEXTURE2D(_SpecularMask);   SAMPLER(sampler_SpecularMask);
            TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
            TEXTURE2D(_BumpMask);        SAMPLER(sampler_BumpMask);
            TEXTURE2D(_CurveMap);       SAMPLER(sampler_CurveMap);
            TEXTURE2D(_SSSRampMap);     SAMPLER(sampler_SSSRampMap);
            TEXTURE2D(_ControlMask1);    SAMPLER(sampler_ControlMask1);
            TEXTURE2D(_ControlMask2);    SAMPLER(sampler_ControlMask2);
            TEXTURE2D(_PaintArea1);    SAMPLER(sampler_PaintArea1);
            TEXTURE2D(_PaintArea2);    SAMPLER(sampler_PaintArea2);
            
            
            
            
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
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv.xy);
                half4 facialPatten = SAMPLE_TEXTURE2D(_FacialPattenTex, sampler_BaseMap, IN.uv.zw);
                half facialPattenMask = SAMPLE_TEXTURE2D(_FacialPattenMask, sampler_FacialPattenMask, IN.uv.zw);
                float4 specularMask = SAMPLE_TEXTURE2D(_SpecularMask, sampler_SpecularMask, IN.uv.xy);
                float curve = SAMPLE_TEXTURE2D(_CurveMap, sampler_CurveMap, IN.uv.xy);
                float smoothMask = SAMPLE_TEXTURE2D(_BumpMask, sampler_BumpMask, IN.uv.zw);
                float3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv.xy), clamp(_BumpScale * (1-smoothMask) * 2, -1, 1));
                float3 normalWS = normalize(TransformTangentToWorld(normalTS, half3x3(IN.tangentWS, IN.bitangentWS, IN.normalWS)));
                half4 mask1 = SAMPLE_TEXTURE2D(_ControlMask1, sampler_ControlMask1, IN.uv.zw);
                half4 mask2 = SAMPLE_TEXTURE2D(_ControlMask2, sampler_ControlMask2, IN.uv.zw);
                //float4 paintArea1 = SAMPLE_TEXTURE2D(_PaintArea1, sampler_PaintArea1, IN.uv);
                //float4 paintArea2 = SAMPLE_TEXTURE2D(_PaintArea2, sampler_PaintArea2, IN.uv);
                
                half3 baseColor = lerp(baseMap.rgb, facialPatten.rgb, facialPatten.a * _FacialPattenIntensity * facialPattenMask);
                
                //绘制区域校正
                
                // mask1.r = adjustPaint(mask1.r);
                // mask1.g = adjustPaint(mask1.g);
                // mask1.b = adjustPaint(mask1.b);
                // mask1.a = adjustPaint(mask1.a);
                // mask2.r = adjustPaint(mask2.r);
                // mask2.g = adjustPaint(mask2.g);
                // mask2.b = adjustPaint(mask2.b);
                // mask2.a = adjustPaint(mask2.a);
                
                
                float alpha = mask1.r + mask1.g + mask1.b + mask1.a + mask2.r + mask2.g + mask2.b + mask2.a;
                
                
                //计算绘制颜色
                half4 paintColor = mul(_MaskColor1, mask1) + mul(_MaskColor2, half4(mask2));
                baseColor = lerp(baseColor, paintColor.rgb, alpha * facialPattenMask.r);
                
                //绘制油彩部分，高光更强
                //TODO：找到更合适的高光表达
                specularMask = saturate(specularMask *smoothMask*2+ 0.1f*smoothMask);
                
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
                
                //自发光
                color.rgb = lerp(color.rgb,facialPatten.rgb,_EmissionScale*facialPatten.a *facialPattenMask);
                //color = half3(alpha,alpha,alpha);
                clip(baseMap.a - _Cutoff);
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

//// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
//
//// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
//
Shader "Custom UI/Rotate Indefinitely"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _Speed ("Speed", Float) = 3.14

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                half4 mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

            float _Speed;

            float2 rotateUV(float2 uv, float rotation, float2 mid)
            {
                return float2(
                    cos(rotation) * (uv.x - mid.x) + sin(rotation) * (uv.y - mid.y) + mid.x,
                    cos(rotation) * (uv.y - mid.y) - sin(rotation) * (uv.x - mid.x) + mid.y
                );
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                // v.vertex.xy = rotateUV(v.vertex.xy, _Speed * _Time.y, (mul(float4(0, 0, 0, 1), UNITY_MATRIX_M).xy));
                float4 vPosition = UnityObjectToClipPos(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;

                float2 pixelSize = vPosition.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);

                // float c = cos(_Time.y * _Speed);
                // float s = sin(_Time.y * _Speed);
                // float2 uv = v.texcoord.xy - 0.5;
                // float2 newUv = 0;
                // newUv.x = c * uv.x - s * uv.y;
                // newUv.y = s * uv.x + c * uv.y;
                // newUv = newUv + 0.5;
                // v.texcoord.xy = newUv;

                v.texcoord.xy = rotateUV(v.texcoord.xy, _Time.y * _Speed, float2(0.5, 0.5));

                OUT.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
                OUT.mask = half4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw,
                                 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = IN.color * (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd);

                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color.rgb *= color.a;

                return color;
            }
            ENDCG
        }
    }
}

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//Shader "Custom UI/Rotate Indefinitely"
//{
//    Properties
//    {
//        [PerRendererData]
//        _MainTex ("Sprite Texture", 2D) = "white" {}
//        _Color ("Tint", Color) = (1,1,1,1)
//        
//        _Speed ("Speed", Float) = 3.14
//    }
//    SubShader
//    {
//        Tags
//        {
//            "Queue"="Transparent"
//            "IgnoreProjector"="True"
//            "RenderType"="Transparent"
//            "PreviewType"="Plane"
//            "CanUseSpriteAtlas"="True"
//        }
//
//        Cull Off
//        Lighting Off
//        ZWrite Off
//        ZTest [unity_GUIZTestMode]
//        Blend SrcAlpha OneMinusSrcAlpha
//
//        Pass
//        {
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//
//            #include "UnityCG.cginc"
//            #include "UnityUI.cginc"
//
//            struct appdata_t
//            {
//                float4 vertex : POSITION;
//                float4 color : COLOR;
//                float2 texcoord : TEXCOORD0;
//            };
//
//            struct v2f
//            {
//                float4 vertex : SV_POSITION;
//                fixed4 color : COLOR;
//                half2 texcoord : TEXCOORD0;
//                float4 worldPosition : TEXCOORD1;
//            };
//
//            fixed4 _Color;
//            fixed4 _TextureSampleAdd;
//
//            float _Speed;
//            
//            float2 rotateUV(float2 uv, float rotation, float2 mid)
//            {
//                return float2(
//                    cos(rotation) * (uv.x - mid.x) + sin(rotation) * (uv.y - mid.y) + mid.x,
//                    cos(rotation) * (uv.y - mid.y) - sin(rotation) * (uv.x - mid.x) + mid.y
//                );
//            }
//
//            v2f vert(appdata_t IN)
//            {
//                v2f OUT;
//                OUT.worldPosition = IN.vertex;
//                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
//                
//                IN.texcoord.xy = rotateUV(IN.texcoord.xy, _Time.y * _Speed, float2(0.5, 0.5));
//
//                OUT.texcoord = IN.texcoord;
//
//                #ifdef UNITY_HALF_TEXEL_OFFSET
//			OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
//                #endif
//
//                OUT.color = IN.color * _Color;
//                return OUT;
//            }
//
//            sampler2D _MainTex;
//            fixed4 frag(v2f IN) : SV_Target
//            {
//                return (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
//            }
//            ENDCG
//        }
//    }
//}
﻿Shader "GPM/GpmDissolve"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _EdgeColor("Edge color", Color) = (1, 1, 1, 1)
        _EdgeWidth("Edge width", Range(0.0, 1.0)) = 0.1
        _Dissolve("Dissolve", Range(0.0, 1.0)) = 0.1

        [HideInInspector] _StencilComp("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        LOD 100

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }
        ColorMask[_ColorMask]
        ZTest[unity_GUIZTestMode]

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            sampler2D _NoiseTex;
            
            float4 _EdgeColor;
            float _EdgeWidth;
            float _Dissolve;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv) * _Color;
                float cutout = tex2D(_NoiseTex, i.uv).r;

                if (cutout <= _Dissolve)
                {
                    color.a = 0;
                }
                else
                {
                    if (cutout < color.a && cutout < _Dissolve + _EdgeWidth)
                    {
                        color = lerp(_EdgeColor, color, (cutout - _Dissolve) / _EdgeWidth);
                    }
                }

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
﻿// Upgrade NOTE: upgraded instancing buffer 'prop' to new syntax.

Shader "Custom/2D/Fluid"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_BucketFillAmount("FillAmount", Range(0,1)) = 1
		_WaveBorderThickness("BorderThickness", Range(0,1)) = 0.05
		_WaveSize("WaveSize", Range(0,.1)) = 0.036
		_WaveSpeed("WaveSpeed", Range(0,10)) = 4
		_WaveAmount("WaveAmount", Range(0,40)) = 20

		_MinFill("MinFill", Range(0, 1)) = 0
		_MaxFill("MaxFill", Range(0,1)) = 0

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Stencil
	{
		Ref[_Stencil]
		Comp[_StencilComp]
		Pass[_StencilOp]
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
	{
		Name "Default"
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0

#include "UnityCG.cginc"
#include "UnityUI.cginc"

#pragma multi_compile __ UNITY_UI_CLIP_RECT
#pragma multi_compile __ UNITY_UI_ALPHACLIP


		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
		float4 worldPosition : TEXCOORD1;
		UNITY_VERTEX_OUTPUT_STEREO
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	UNITY_INSTANCING_BUFFER_START(prop)
		UNITY_DEFINE_INSTANCED_PROP(float, _BucketFillAmount)
#define _BucketFillAmount_arr prop
	UNITY_INSTANCING_BUFFER_END(prop)

	fixed4 _Color;
	fixed4 _TextureSampleAdd;
	float4 _ClipRect;

	v2f vert(appdata_t v)
	{
		v2f OUT;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
		UNITY_TRANSFER_INSTANCE_ID(v, o);

		OUT.worldPosition = v.vertex;
		
		OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
		OUT.texcoord = v.texcoord;

		OUT.color = v.color * _Color;
		return OUT;
	}

	sampler2D _MainTex;
	float _WaveSize;
	float _WaveSpeed;
	float _WaveAmount;
	float _WaveBorderThickness;

	float _MaxFill;
	float _MinFill;

	fixed4 frag(v2f IN) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(i);
		float fillAmount = UNITY_ACCESS_INSTANCED_PROP(_BucketFillAmount_arr, _BucketFillAmount) / 2;
		half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
		
		float wave = sin(_Time.y * _WaveSpeed + IN.texcoord.x * _WaveAmount);
		half waveHeight = fillAmount + lerp(fillAmount - _WaveSize, fillAmount, wave);
		waveHeight = (_MaxFill - _MinFill) * waveHeight + _MinFill; //"clamp" if the visible image is smaller than the source
		
		float dist = waveHeight - IN.texcoord.y;	 
		color.rgb -= float3(.2, .2, .2) * max(0, sign(_WaveBorderThickness - dist)); // darker outline on the top of the wave
		color.a = color.a * max(0, sign(waveHeight - IN.texcoord.y)); // keep source alpha but make all fill above the wave invisible

		#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
		#endif

		#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
		#endif

		return color;
		}
			ENDCG
		}
	}
}
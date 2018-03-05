// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Custom/VerticsOutline_Always"
{
	Properties{
		_Color("Color", Color) = (0,1,0,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_OutlineFactor("Outline Factor", Range(0, 1)) = 1
		_OutlineColor("Outline Color", Color) = (0.67,1,0.184,1)
		_OutlineWidth("Outline Width", Range(0, 10)) = .1
		_BodyAlpha("Body Alpha", Range(0, 1)) = 1

		_Stencil("Stencil ID", Int) = 16

		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255
	}

		SubShader{
		Tags{
		"RenderType" = "Transparent"
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
	}
		LOD 200

		Pass{
		Name "VerticsOutline_Outline_Stencil"

		Cull Off
		ZWrite Off
		ZTest Always
		ColorMask 0

		Stencil{
		Ref[_Stencil]
		Comp Always
		Pass Replace
		ZFail Replace
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_FOG_COORDS(1)
			UNITY_VERTEX_OUTPUT_STEREO
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		UNITY_TRANSFER_FOG(o, o.vertex);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.texcoord);
	UNITY_APPLY_FOG(i.fogCoord, col);
	UNITY_OPAQUE_ALPHA(col.a);


	return col;
	}

		ENDCG
	}

		Pass{
		Name "VerticsOutline_Outline_Face1"

		Cull Off
		ZWrite On
		ZTest Always
		ColorMask RGBA

		Stencil{
		Ref[_Stencil]
		Comp NotEqual
		Pass Keep
		ZFail Keep
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_FOG_COORDS(1)
			UNITY_VERTEX_OUTPUT_STEREO
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	uniform fixed4 _OutlineColor;
	uniform float _OutlineWidth;
	uniform float _OutlineFactor;

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		float3 caculateVec = lerp(normalize(v.vertex.xyz), normalize(v.normal), _OutlineFactor);

		o.vertex = UnityObjectToClipPos(v.vertex);
		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, caculateVec);
		float2 offset = TransformViewToProjection(norm.xy);
		o.vertex.xy += offset * o.vertex.z * _OutlineWidth;

		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = _OutlineColor;

	UNITY_APPLY_FOG(i.fogCoord, col);
	UNITY_OPAQUE_ALPHA(col.a);
	return col;
	}

		ENDCG
	}

		Pass{
		Name "VerticsOutline_Outline_Face2"

		Cull Off
		ZWrite On
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Stencil{
		Ref[_Stencil]
		Comp NotEqual
		Pass Keep
		ZFail Keep
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_FOG_COORDS(1)
			UNITY_VERTEX_OUTPUT_STEREO
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	uniform fixed4 _OutlineColor;
	uniform float _OutlineWidth;
	uniform float _OutlineFactor;

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		float3 caculateVec = -lerp(normalize(v.vertex.xyz), normalize(v.normal), _OutlineFactor);

		o.vertex = UnityObjectToClipPos(v.vertex);
		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, caculateVec);
		float2 offset = TransformViewToProjection(norm.xy);
		o.vertex.xy += offset * o.vertex.z * _OutlineWidth;

		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = _OutlineColor;

	UNITY_APPLY_FOG(i.fogCoord, col);
	UNITY_OPAQUE_ALPHA(col.a);
	return col;
	}

		ENDCG
	}

		Pass{
		Name "VerticsOutline_Body"

		Cull Back
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGBA

		Stencil{
		Ref[_Stencil]
		Comp Always
		Pass Replace
		ZFail Replace
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_FOG_COORDS(1)
			UNITY_VERTEX_OUTPUT_STEREO
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	uniform float _BodyAlpha;

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.vertex = UnityObjectToClipPos(v.vertex);

		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 _Color;

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.texcoord);
	UNITY_APPLY_FOG(i.fogCoord, col);
	UNITY_OPAQUE_ALPHA(col.a);
	col *= _Color;
	return fixed4(col.rgb, _BodyAlpha);
	}

		ENDCG
	}
	}
}










Shader "Custom/OutLineSurf" 
{
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		//(0.67,1,0.184,1)
		_OutlineColor("Outline Color", Color) = (1,0,0,1)
		_Outline("Outline width", Range(.002, 10)) = 0.2
		_MainTex("Base (RGB)", 2D) = "white" { }
	}

		CGINCLUDE
#include "UnityCG.cginc"

		struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR;
	};

	uniform float _Outline;
	uniform float4 _OutlineColor;

	v2f vert(appdata v) {
		// just make a copy of incoming vertex data but scaled according to normal direction
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		float2 offset = TransformViewToProjection(norm.xy);

		o.pos.xy += offset * o.pos.z * _Outline;
		o.color = _OutlineColor;
		return o;
	}
	ENDCG

		SubShader{
		//Tags {"Queue" = "Overlay" }
		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _MainTex;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG

		// note that a vertex shader is specified here but its using the one above
		Pass{
		Name "OUTLINE"
		Tags{ "LightMode" = "Always" "Queue" = "Overlay" }
		Cull Front
		ZWrite On
		ZTest LEqual
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha
		Offset 15,15

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		half4 frag(v2f i) :COLOR{ return i.color; }
		ENDCG
	}
	}

		SubShader{
		Tags{ "Queue" = "Overlay" }
		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _MainTex;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG

		Pass{
		Name "OUTLINE"
		Tags{ "LightMode" = "Always" }
		Cull Front
		ZWrite On
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert
#pragma exclude_renderers gles xbox360 ps3
		ENDCG
		SetTexture[_MainTex]{ combine primary }
	}
	}
	FallBack "Diffuse"
}
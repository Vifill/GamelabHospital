﻿Shader "Custom/Outline/Outline"
{
	Properties{
	_Color("Color", Color) = (1.000000,1.000000,1.000000,1.000000)
	_MainTex("Albedo", 2D) = "white" { }
	_Cutoff("Alpha Cutoff", Range(0.000000,1.000000)) = 0.500000
	_Glossiness("Smoothness", Range(0.000000,1.000000)) = 0.500000
	_GlossMapScale("Smoothness Scale", Range(0.000000,1.000000)) = 1.000000
	[Enum(Metallic Alpha,0,Albedo Alpha,1)]  _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0.000000
	[Gamma]  _Metallic("Metallic", Range(0.000000,1.000000)) = 0.000000
	_MetallicGlossMap("Metallic", 2D) = "white" { }
	[ToggleOff]  _SpecularHighlights("Specular Highlights", Float) = 1.000000
	[ToggleOff]  _GlossyReflections("Glossy Reflections", Float) = 1.000000
	_BumpScale("Scale", Float) = 1.000000
	_BumpMap("Normal Map", 2D) = "bump" { }
	_Parallax("Height Scale", Range(0.005000,0.080000)) = 0.020000
	_ParallaxMap("Height Map", 2D) = "black" { }
	_OcclusionStrength("Strength", Range(0.000000,1.000000)) = 1.000000
	_OcclusionMap("Occlusion", 2D) = "white" { }
	_EmissionColor("Color", Color) = (0.000000,0.000000,0.000000,1.000000)
	_EmissionMap("Emission", 2D) = "white" { }
	_DetailMask("Detail Mask", 2D) = "white" { }
	_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" { }
	_DetailNormalMapScale("Scale", Float) = 1.000000
	_DetailNormalMap("Normal Map", 2D) = "bump" { }
	[Enum(UV0,0,UV1,1)]  _UVSec("UV Set for secondary textures", Float) = 0.000000
		



		//_Color("Color", Color) = (1,1,1,1)
		//_MainTex("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness("Smoothness", Range(0,1)) = 0.5
		//_Metallic("Metallic", Range(0,1)) = 0.0

		_Brightness("Brightness", Range(0, .1)) = 0.03
		_OutlineFactor("Outline Factor", Range(0, 1)) = 1
		_OutlineColor("Outline Color", Color) = (0.67,1,0.184,1)
		_OutlineWidth("Outline Width", Range(0, 10)) = .15
		_BodyAlpha("Body Alpha", Range(0, 1)) = 1

		_Stencil("Stencil ID", Int) = 16

		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255
		[HideInInspector]  _Mode("__mode", Float) = 0.000000
		[HideInInspector]  _SrcBlend("__src", Float) = 1.000000
		[HideInInspector]  _DstBlend("__dst", Float) = 0.000000
		[HideInInspector]  _ZWrite("__zw", Float) = 1.000000

	}
	SubShader {
		Tags { 
			"RenderType" = "Opaque" 
			

		}
		LOD 200

		Pass {
			Name "VerticsOutline_Outline_Stencil"

			Cull Off
			ZWrite Off
			ZTest Always
			ColorMask 0

			Stencil {
				Ref [_Stencil]
				Comp Always
				Pass Replace
				ZFail Replace
				ReadMask [_StencilReadMask]
				WriteMask [_StencilWriteMask]
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

		Pass {
			Name "VerticsOutline_Outline_Face1"

			Cull Off
			ZWrite On
			ZTest Always
			ColorMask RGBA
			
			Stencil {
				Ref [_Stencil]
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

		Pass {
			Name "VerticsOutline_Outline_Face2"

			Cull Off
			ZWrite On
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

			Stencil {
				Ref [_Stencil]
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
		

			CGPROGRAM
#pragma surface surf Standard fullforwardshadows finalcolor:col
#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		float _Brightness;
		float _Glossiness;
		float _Metallic;
		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void col(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			float brightness = _Brightness;
			color += brightness;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

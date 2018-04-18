Shader "Custom/Pulse"
{
	Properties{
		_Color("Color", Color) = (0,1,0,1)
		_TintColor("Tint", color) = (0.9,1,0.9,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_Brightness("Brightness", Range(0, .1)) = 0.03
		_BodyAlpha("Body Alpha", Range(0, 1)) = 1

		_PulseSpeed("PulseSpeed", Range(0, 20)) = 4
		_PulseAmount("PulseAmount", Range(0,.1)) = .04
		_Stencil("Stencil ID", Int) = 16

		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		

		CGPROGRAM
#pragma surface surf Standard fullforwardshadows finalcolor:col
#pragma target 3.0

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
	};

	float _Brightness;
	half _Glossiness;
	half _Metallic;
	fixed4 _Color;
	fixed4 _TintColor;
	float _PulseSpeed;
	float _PulseAmount;

	UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void col(Input IN, SurfaceOutputStandard o, inout fixed4 color)
	{
		float brightness = _Brightness;
		brightness = step(0 , _PulseAmount) * sin(_Time.y * _PulseSpeed) * _PulseAmount;
		color = (color * _TintColor) + brightness;
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

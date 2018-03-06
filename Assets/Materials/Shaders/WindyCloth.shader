Shader "Custom/WindyCloth" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_WindAmount("WindAmount", Float) = 1.0
		_WindWavesAmount("WindWavesAmount", Float) = 1.0
		_Phase("Phase", Float) = 20.0
		_RandomOffset("RandomOffset", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(float, _RandomOffset)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Bounds[8])
		UNITY_INSTANCING_BUFFER_END(Props)
		
		half _WindAmount;
		half _WindWavesAmount;
		half _Phase;
		void vert(inout appdata_full v) 
		{
			float randomOffset = UNITY_ACCESS_INSTANCED_PROP(Props, _RandomOffset);
			float phase = _Time.y * _Phase;
			v.vertex.xyz += (v.normal * abs(sin(phase * _RandomOffset + v.vertex.y * _WindWavesAmount)) * _WindAmount);
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

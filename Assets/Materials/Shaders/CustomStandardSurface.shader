Shader "Custom/Testing/CustomStandardSurface" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {}
		_MetallicGlossMap("Metallic", 2D) = "white" { }
		_Metallic("Metallic", Range(0,1)) = 0.0
		[Normal] _BumpMap("Normal", 2D) = "bump" {}
		_BumpScale("Normal Scale", Float) = 0.0
		_ParallaxMap("Height", 2D) = "black" {}
		_Parallax("Height Scale", Range(0,.2)) = 0.0
		_Detail("Detail", 2D) = "gray" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Detail;
		sampler2D _ParallaxMap;
		sampler2D _MetallicGlossMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_Detail;
			float2 uv_MetallicGlossMap;
		};

		half _Glossiness;
		half _Metallic;
		half _Parallax;
		float _BumpScale;

		fixed4 _Color;

	
		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float4 heightMap = tex2Dlod(_ParallaxMap, float4(v.texcoord.xy, 0, 0));
			v.vertex.xyz += v.normal * heightMap.b * _Parallax;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Albedo *= tex2D(_Detail, IN.uv_Detail).rgb * 2;
			o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MetallicGlossMap).rgb * _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap ));
			o.Normal.xy *= _BumpScale;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

Shader "Custom/FakeWindow" {
	Properties {
		_Normal ("Normal", 2D) = "bump" {}
		_Tex1 ("Layer1", 2D) = "white" {}
		_Tex2 ("Layer2", 2D) = "white" {}
		_Tex3 ("Layer3", 2D) = "white" {}
		_Tex4 ("Layer4", 2D) = "white" {}
		_LayerDistance ("Layer Distance", Vector) = (0,0,0,0)
		_Color ("Color Tint", Vector) = (1,1,1,1)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
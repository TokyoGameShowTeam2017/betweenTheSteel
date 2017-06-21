Shader "Fujimaki/Robo" {
	Properties{
		_MainTex ( "Texture", 2D ) = "white" {}
		_Normal ( "Normal",2D ) = "bump"{}
		_Distance ( "AlphaDistance",Float ) = 2.5
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM

		sampler2D _MainTex;
		sampler2D _Normal;
		float _Distance;
		sampler3D	_DitherMaskLOD;


	#pragma surface surf Lambert

	struct Input
	{
		float2 uv_MainTex;
		float3 worldPos;
		float4 screenPos;
	};
	void surf ( Input IN, inout SurfaceOutput o )
	{
		// ディザリングで半透明を表現
		half alphaRef = tex3D ( _DitherMaskLOD, float3(IN.screenPos.xy / IN.screenPos.w * _ScreenParams.xy * 0.25,clamp( (_Distance),0.0,0.99)) ).a;
		clip ( alphaRef - 0.01 );

		o.Albedo = tex2D ( _MainTex, IN.uv_MainTex).rgb;
		o.Normal = tex2D ( _Normal, IN.uv_MainTex );
	}
	ENDCG
	}
		Fallback "Diffuse"
}
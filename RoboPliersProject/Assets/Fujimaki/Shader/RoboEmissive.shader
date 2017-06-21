Shader "Fujimaki/RoboEmission" {
	Properties{
		_MainTex ( "MaskTex", 2D ) = "white" {}
		_Color ( "Main Color", Color ) = (1,1,1,1)
		_Distance ( "AlphaDistance",Float ) = 2.5
		_EmissionPower ( "EmissionPower",Float ) = 1
		_Speed ( "Speed",Float ) = 0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM

		fixed4 _Color;
		float _Distance;
		float _EmissionPower;
		float _Speed;
		sampler2D _MainTex;
		sampler3D _DitherMaskLOD;


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
		half alphaRef = tex3D ( _DitherMaskLOD, float3(IN.screenPos.xy / IN.screenPos.w * _ScreenParams.xy * 0.25,clamp ( (_Distance),0.0,0.99 )) ).a;
		clip ( alphaRef - 0.01 );

		o.Albedo = _Color.rgb;
		o.Emission = _Color.rgb*pow ( tex2D ( _MainTex, IN.uv_MainTex + float2(0, _Time.y*_Speed) ).r, 2 )*_EmissionPower;
	}
	ENDCG
	}
		Fallback "Diffuse"
}
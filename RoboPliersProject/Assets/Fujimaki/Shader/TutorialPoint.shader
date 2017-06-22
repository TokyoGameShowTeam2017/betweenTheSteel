Shader "Fujimaki/TutorialPoint" {
	Properties{
		_MainTex ( "Texture", 2D ) = "white" {}
		_Color ( "Main Color", Color ) = (1,1,1,1)
		_RimColor ( "Rim Color", Color ) = (1,1,1,1)
		_RimPower ( "Rim Power", Float ) = 0
		_RimSize ( "Rim Size", Float ) = 0
		_RimUnderLimit ( "Rim UnderLimit",Float ) = 0
		_EmissionPower ( "EmissionPower",Float ) = 1
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
	}
		Cull off

		CGPROGRAM
#pragma surface surf Lambert alpha

		fixed4 _Color;
	float4 _RimColor;
	float _RimPower;
	float _RimSize;
	float _RimUnderLimit;
	float _EmissionPower;
	sampler2D _MainTex;

	struct Input
	{
		float3 worldNormal;
		float3 viewDir;
		float2 uv_MainTex;

	};

	void surf ( Input IN, inout SurfaceOutput o )
	{
		o.Emission = _Color.rgb*_EmissionPower;

		float alpha = 1 - (abs ( dot ( IN.viewDir, IN.worldNormal ) ));
		float texAlpha = tex2D ( _MainTex, IN.uv_MainTex ).a*clamp ( sin ( _Time.z )*0.5 + 0.8, 0, 1 );
		o.Alpha = clamp ( pow ( alpha, _RimSize )*_RimPower + _RimUnderLimit, 0, 1 )*texAlpha;
	}
	ENDCG
	}
}
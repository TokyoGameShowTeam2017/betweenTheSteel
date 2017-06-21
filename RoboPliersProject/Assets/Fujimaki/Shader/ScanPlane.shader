Shader "Fujimaki/Scan" {
	Properties
	{
		_Color ( "Main Color", Color ) = (1,1,1,1)
		_RimColor ( "Rim Color", Color ) = (1,1,1,1)
		_RimPower ( "Rim Power", Float ) = 0
		_RimSize ( "Rim Size", Float ) = 0
		_RimUnderLimit ( "Rim UnderLimit",Float ) = 0
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
	sampler2D _MaskTex;

	struct Input
	{
		float3 worldNormal;
		float3 viewDir;
		float2 uv_MainTex;
	};

	void surf ( Input IN, inout SurfaceOutput o )
	{
		o.Emission = _Color.rgb*1.2;

		float alpha = 1 - (abs ( dot ( IN.viewDir, IN.worldNormal ) ));
		o.Alpha = clamp ( pow ( alpha, _RimSize )*_RimPower + _RimUnderLimit, 0, 1 );
	}
	ENDCG
	}
}
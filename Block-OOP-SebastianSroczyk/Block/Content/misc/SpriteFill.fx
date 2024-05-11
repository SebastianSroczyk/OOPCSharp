#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float effectStrength;
float4 fillColour;

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
	//color.rgb = input.Color.rgb;
	color.x = lerp(color.x * input.Color.x, fillColour.x, effectStrength);
	color.y = lerp(color.y * input.Color.y, fillColour.y, effectStrength);
	color.z = lerp(color.z * input.Color.z, fillColour.z, effectStrength);
	if(color.w != 0)
		color.w = lerp(color.w * input.Color.w, fillColour.w, effectStrength);
	return color;
}

/*
float4 MainPS(float2 textureCoordinates: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(inputTexture, textureCoordinates) * tintColour;

	color.x = lerp(color.x, fillColour.x, effectStrength);
	color.y = lerp(color.y, fillColour.y, effectStrength);
	color.z = lerp(color.z, fillColour.z, effectStrength);
	return color;
}
//*/


technique Techninque1
{
	pass Pass1
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
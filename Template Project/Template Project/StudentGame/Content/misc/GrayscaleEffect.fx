// Heavily inspired by the code written by BenMakesGames: https://mysteriousspace.com/2019/01/05/pixel-shaders-in-monogame-a-tutorial-of-sorts-for-2019/
#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler inputTexture;
float grayscaleStrength;

float4 MainPS(float2 textureCoordinates: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(inputTexture, textureCoordinates);
	float4 grayscale = color.r * 0.2126 + color.g * 0.7152 + color.b * 0.0722;
	
	color.x = lerp(grayscale.x, color.x, grayscaleStrength);
	color.y = lerp(grayscale.y, color.y, grayscaleStrength);
	color.z = lerp(grayscale.z, color.z, grayscaleStrength);
	return color;
}

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
#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


float Time;
matrix WorldViewProjection;

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

Texture2D Texture2;
sampler2D SpriteTextureSampler2 = sampler_state
{
	Texture = <Texture2>;
};

SamplerState Sampler;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	//0.21 R + 0.72 G + 0.07 B
	float4 colorScale = input.TextureCoordinates.y;
	float4 colorModifier = float4(1, 1, 1, 1) * colorScale;
	colorModifier.w = 1;

	float4 originalColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float grey = originalColor.r * .21 + originalColor.g * .72 + originalColor.b * .07;
	float4 finalColor = float4(grey, grey, grey, 1);


	
	float scrunchLocation = abs(sin(Time));
	float closeness = 0;

	if (input.TextureCoordinates.x - scrunchLocation != 0)
	{
		closeness = 0.0010 / (scrunchLocation - input.TextureCoordinates.x);
	}
	float4 lavaColor = SpriteTexture.Sample(Sampler, input.TextureCoordinates);
	return Texture2.Sample(Sampler, input.TextureCoordinates).a * lavaColor;

	//return tex2D(SpriteTextureSampler2, input.TextureCoordinates + float2(closeness, 0)) * (input.Color);


	//if (input.TextureCoordinates.x > scrunchLocation && input.TextureCoordinates.x < scrunchLocation + .005)
	//{
	//	return float4(0, 0, 0, 1);
	//}
	//else
	//{
	//	return tex2D(SpriteTextureSampler, input.TextureCoordinates + float4(closeness, closeness)) * input.Color;
	//}
}

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.Color = input.Color;

	return output;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
		VertexShader = compile VS_SHADERMODEL MainVS();
	}
};
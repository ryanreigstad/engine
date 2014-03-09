#version 410

layout(location = 0) in vec4 Color;

layout(location = 0) out vec4 FragColor;

void main()
{
	FragColor = Color//; cause a syntax error so that it doesn't use the shader because it's still broken on mine
}
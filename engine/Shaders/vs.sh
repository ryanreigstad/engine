#version 410

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec4 vColor;

out vec4 Color;

void main()
{
	Color = vColor;
	gl_Position = vec4(vPosition, 0.0);
}
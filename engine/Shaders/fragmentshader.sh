#version 330
out vec4 outputColor;
layout(location = 1) in vec4 vColor;
void main()
{
    outputColor = vColor;
}
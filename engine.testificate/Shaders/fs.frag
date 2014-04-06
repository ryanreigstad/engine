#version 410

uniform sampler2D ObjectTexture;

in Data {
	vec3 Position;
	vec3 Normal;
	vec2 Texture;
} vdata;

out vec4 FragColor;

void main()
{
	vec4 c = texture(ObjectTexture, vdata.Texture);
	FragColor = c;
}
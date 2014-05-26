#version 440

in vec2 gl_FragCoord;

uniform sampler2D ObjectTexture;

struct Data {
	vec3 Position;
	vec3 Normal;
	vec2 Texture;
};

in Data vdata;

layout(location = 0) out vec4 Position;
layout(location = 1) out vec4 Normal;
layout(location = 2) out vec4 Texture;

void main () {
	Position = vec4(vdata.Position, 0.0);
	Normal = vec4(vdata.Normal, 0.0);
	Texture = texture(ObjectTexture, vdata.Texture);
}
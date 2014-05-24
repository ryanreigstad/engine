#version 440

uniform mat4 ViewMatrix;
uniform mat4 ModelMatrix;
uniform mat4 ModelRotationMatrix;
uniform mat4 ModelViewMatrix;

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec3 vNormal;
layout(location = 2) in vec2 vTexture;

struct Data {
	vec3 Position;
	vec3 Normal;
	vec2 Texture;
};

out Data vdata;

void main () {
	vdata.Position = vPosition;
	vdata.Normal = normalize(vNormal);
	vdata.Texture = vTexture;

	gl_Position = ModelViewMatrix * vec4(vPosition, 1.0);
}
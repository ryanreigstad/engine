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
	vec4 p = ModelViewMatrix * vec4(vPosition, 1.0);

	vdata.Position = vec3(p);
	vdata.Normal = vec3(normalize(ModelRotationMatrix * vec4(vNormal, 1)));
	vdata.Texture = vTexture;

	gl_Position = p;
}
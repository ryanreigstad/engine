#version 440

in vec2 gl_FragCoord;

uniform mat4 ModelViewMatrix;

uniform sampler2D PositionTexture;
uniform sampler2D NormalTexture;
uniform sampler2D ColorTexture;

uniform vec3 LightPosition;
uniform vec3 LightSpecularity;
uniform vec3 LightDiffuse;

//uniform vec3 Kd; //material Diffuse reflectivity
//uniform vec3 Ld; //Light source intensity

out vec4 FragColor;

const vec2 screensize = vec2(1600, 900);
void main () {
	vec2 ss = gl_FragCoord / screensize;

	vec4 position = texture (PositionTexture, ss);
	vec4 normal = texture (NormalTexture, ss);
	vec4 color = texture (ColorTexture, ss);
  
	// TODO: this.
	vec3 s = normalize(LightPosition - position.xyz);

	float LightIntensity = 1.0 * 1.0 * max( dot(s, normal.xyz), 0.1 );

	FragColor = color * LightIntensity;

	//FragColor = vec4(LightIntensity, LightIntensity, LightIntensity, 1.0);// color + position / 1000.0f + normal / 100.0f;
}
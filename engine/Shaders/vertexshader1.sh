#version 410

// Layout
layout(location = 0) in vec3 position;
layout(location = 1) in vec4 color;
layout(location = 2) in vec3 normal;
layout(location = 3) in vec2 texCoords;
 
// Includes
// matrices for the active camera
uniform mat4 MVP;
uniform mat4 Model;
uniform mat4 View;
uniform mat4 ModelRotation;

uniform vec3 LightPos;
uniform vec3 AmbientLightColor;

//Outputs
out vec2 texCoord;
out vec4 outColor;
///////////////////////////////////////////////////////////////////////////////
 
vec4 GetAmbient();
vec4 GetDiffused();

void main()
{
    gl_Position = (MVP * vec4(position, 1.0));
	texCoord = texCoords;
	vec4 test = GetAmbient();
	vec4 test1 = GetDiffused();
	outColor = color * test1;
}

vec4 GetDiffused()
{
	mat3 mat = mat3(ModelRotation);
	//get the normal relative to the model
	vec3 worldSpaceModelNormal = mat * normal;

	//get the vertex relative to where is is in the world
	vec3 vertexWorldPosition = vec3(Model * vec4(position, 1.0f) );

	//get the vector where the light's position is.
	vec3 lightVector = normalize( LightPos - vertexWorldPosition );

	//brightness is 0 to 1 based on the angle between the two vectors
	float diffuseBrightness = dot(worldSpaceModelNormal, lightVector);

	//clamp the brightness
	diffuseBrightness = clamp(diffuseBrightness, 0.0f, 1.0f);

	return vec4(diffuseBrightness, diffuseBrightness, diffuseBrightness, 1);
}

vec4 GetAmbient()
{
	return vec4(AmbientLightColor, 1.0f);
}


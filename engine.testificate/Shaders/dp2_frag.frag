#version 440

uniform mat4 ModelViewMatrix;

uniform sampler2D PositionTexture;
uniform sampler2D NormalTexture;
uniform sampler2D TextureTexture;

uniform vec3 LightPosition;

//uniform vec3 Kd; //material Diffuse reflectivity
//uniform vec3 Ld; //Light source intensity

uniform vec3 LightSpecularity;
uniform vec3 LightDiffuse;

out vec4 FragColor;

const vec2 screensize = vec2(1600, 900);
void main () {
  vec2 ss;
  ss.s = gl_FragCoord.x / screensize.x;
  ss.t = gl_FragCoord.y / screensize.y;

  vec4 position = texture (PositionTexture, ss);
  vec4 normal = texture (NormalTexture, ss);
  vec4 color = texture (TextureTexture, ss);
  
  // TODO: this.
  vec3 s = normalize(vec3(LightPosition - position.xyz));

  float LightIntensity = 1.0 * 1.0 * max( dot(s, normal.xyz), 0.1 );

  FragColor = color * LightIntensity;

  //FragColor = vec4(LightIntensity, LightIntensity, LightIntensity, 1.0);// color + position / 1000.0f + normal / 100.0f;
}
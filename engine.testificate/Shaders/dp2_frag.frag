#version 440

uniform mat4 ModelViewMatrix;

uniform sampler2D PositionTexture;
uniform sampler2D NormalTexture;
uniform sampler2D TextureTexture;

uniform vec3 LightPosition;
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

  FragColor = color + position / 1000.0f + normal / 100.0f;
}
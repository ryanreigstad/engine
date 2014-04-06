#version 440

uniform mat4 ModelViewMatrix;

uniform sampler2D PositionTexture;
uniform sampler2D NormalTexture;
uniform sampler2D TextureTexture;

uniform vec3 LightPosition;
uniform vec3 LightSpecularity;
uniform vec3 LightDiffuse;

out vec4 FragColor;

void main () {
// todo: real tex cords
  vec2 ss;
  ss.s = gl_FragCoord.x / 1600.0;
  ss.t = gl_FragCoord.y / 900.0;

  vec4 position = texture (PositionTexture, ss);
  vec4 normal = texture (NormalTexture, ss);
  vec4 color = texture (TextureTexture, ss);
  
  // TODO: this.

  FragColor = color;
}
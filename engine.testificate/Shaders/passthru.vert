#version 440

uniform mat4 ModelViewMatrix;

layout(location = 0) in vec3 vPosition;

void main () {
  gl_Position = ModelViewMatrix * vec4(vPosition, 1.0);
}
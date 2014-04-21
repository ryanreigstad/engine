﻿#version 440

uniform mat4 ViewMatrix;
uniform mat4 ModelMatrix;
uniform mat4 ModelRotationMatrix;
uniform mat4 ModelViewMatrix;

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec3 vNormal;
layout(location = 2) in vec2 vTexture;

void main () {
  gl_Position = ModelViewMatrix * vec4(vPosition, 1.0);
}
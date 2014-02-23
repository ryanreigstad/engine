///////////////////////////////////////////////////////////////////////////////
#version 330

// Layout
layout(location = 0) in vec3 position;
layout(location = 2) in vec3 normal;
 
// Includes
// matrices for the active camera
uniform mat4 projection;
uniform mat4 modelView;
uniform mat4 MVP;

//Outputs

///////////////////////////////////////////////////////////////////////////////
 

void main()
{
    gl_Position = (MVP * vec4(position, 1.0));

}
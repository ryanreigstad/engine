///////////////////////////////////////////////////////////////////////////////
#version 330
///////////////////////////////////////////////////////////////////////////////
 
// Inputs
in vec2 oTexCoord;
 
layout( location = 0 ) out vec4 oFragColor;
 
void main()
{
    float c = tiles[2];
    vec4 dt1 = texture2D(diffuseTexture, oTexCoord) * diffuseColor;
    vec4 dt2 = vec4(0, 0, 0, 0.84);
    vec4 dt3 = texture2D(diffuse2Texture, oTexCoord2);
    oFragColor = mix(dt2, dt1, dt1.a );
}
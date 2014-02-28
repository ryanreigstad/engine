//////////////////////////////////////////////////
#version 330
//////////////////////////////////////////////////
/////Layouts//////////////////////////////////////
layout(location = 1) in vec4 vColor;
layout(location = 3) in vec2 texCoords;


/////Uniforms/////////////////////////////////////
uniform sampler2D Tex1;



/////Outputs//////////////////////////////////////
out vec4 outputColor;


void main()
{
	
	vec4 tex1Color = texture( Tex1, texCoords );

	if ( tex1Color.a < 0.15 )
		discard;
	else
	{
		if ( gl_FrontFacing )
		{
			outputColor = tex1Color;
		}
		else
		{
			outputColor = -tex1Color;
		}
	}

	outputcolor = vec4(1,1,1,1);
}
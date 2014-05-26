#version 440

in vec2 gl_FragCoord;

uniform sampler2D ColorTexture;

out vec4 FragColor;

const vec2 screensize = vec2(1600, 900);
void main () {
	if (distance(gl_FragCoord.xy, screensize / 2.0) > 100)
		discard;

	vec2 ss = gl_FragCoord / screensize;
	vec4 color = texture(ColorTexture, ss);

	color.r = 0.7 - color.r;
	color.g = 0.7 - color.g;
	color.b = 0.7 - color.b;
	color.a = 1;
	FragColor = color;
}
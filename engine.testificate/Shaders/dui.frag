#version 440

uniform sampler2D ColorTexture;

out vec4 FragColor;

const vec2 screensize = vec2(1600, 900);
void main () {
	if (distance(vec2(gl_FragCoord.x, gl_FragCoord.y), vec2(screensize.x, screensize.y) / 2.0) > 400)
		discard;

	vec2 ss;
	ss.s = gl_FragCoord.x / screensize.x;
	ss.t = gl_FragCoord.y / screensize.y;

	vec4 color = texture(ColorTexture, ss);
	//color.r = 0.7 - color.r;
	//color.g = 0.7 - color.g;
	//color.b = 0.7 - color.b;
	//color.a = 1;
	FragColor = color;
}
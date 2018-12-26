
#version 330 core
layout(location=0) in vec3 aPos;
layout(location=1) in vec3 color;
layout(location=2) in vec2 aTexcoord;

out vec3 vertexColor;
out vec2 Texcoord;

uniform mat4 trans;
uniform float offsetx;

void main()
{
	Texcoord=aTexcoord;
	vertexColor=color;
	gl_Position=trans*vec4(aPos,1.0);
}

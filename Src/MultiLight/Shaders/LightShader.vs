
#version 330 core
layout(location=0) in vec3 aPos;
layout(location=1) in vec3 color;
layout(location=2) in vec2 aTexcoord;
layout(location=3) in vec3 aNormal;

out vec2 Texcoord;
out vec3 Normal;
out vec3 fragWorldPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform float offsetx;

void main()
{
	//裁剪变换
	gl_Position=projection*view*model*vec4(aPos,1.0);
	//将顶点坐标转换到世界坐标系下
	fragWorldPos=(model*vec4(aPos,1.0)).xyz;
	Normal=mat3(transpose(inverse(model)))*aNormal;
	Texcoord=aTexcoord;
	//Normal=(model*vec4(aNormal,1.0)).xyz;
	//Normal=aNormal;
}


#version 450 core
in vec3 vertexColor;
in vec2 Texcoord;

out vec4 FragColor;

void main()
{
	FragColor=vec4(1.0,1.0,1.0,1.0);
	//FragColor=texture(bgTex,Texcoord);
}

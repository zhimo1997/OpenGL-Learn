#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 3) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

out vec3 fragWorldPos;
out vec3 Normal;
out vec2 Texcoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    fragWorldPos = vec3(model * vec4(aPos, 1.0));
    Normal = mat3(transpose(inverse(model))) * aNormal;  
    Texcoord = aTexCoords;
    
    gl_Position = projection * view * vec4(fragWorldPos, 1.0);
}

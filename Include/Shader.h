#pragma once
#include <glad/glad.h>
#include <glm/glm.hpp>
#include <string>
#include <fstream>
#include <sstream>
#include <iostream>

using namespace std;


class Shader
{
public:
	//着色器程序ID
	GLuint ID;

	//构造器读取并创建着色器程序
	Shader(const GLchar* vertexPath,const GLchar* fragmentPath);

	void Use();
	//uniform工具函数
	void SetBool(const string& name,bool value) const;
	void SetInt(const string& name, int value) const;
	void SetFloat(const string& name, float value) const;
	void SetVec3(const string& name, const glm::vec3 &value) const;
	void SetVec3(const string& name, float x,float y,float z) const;
	void SetVec4(const string& name, const glm::vec4 &value) const;
	void SetVec4(const string& name, float x, float y, float z, float w) const;
	void SetMat4(const string& name,const glm::mat4 &mat) const;
};


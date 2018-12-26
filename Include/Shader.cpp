#include "Shader.h"



Shader::Shader(const GLchar* vertexPath, const GLchar* fragmentPath)
{
	//首先要根据路径获取文本里的着色器源码字符串
	string vertexCode;
	string fragmentCode;

	ifstream vShaderFile;
	ifstream fShaderFile;
	vShaderFile.exceptions(ifstream::failbit|ifstream::badbit);
	fShaderFile.exceptions(ifstream::failbit | ifstream::badbit);

	try
	{
		vShaderFile.open(vertexPath);
		fShaderFile.open(fragmentPath);
		stringstream vShaderStream,fShaderStream;

		vShaderStream << vShaderFile.rdbuf();
		fShaderStream << fShaderFile.rdbuf();

		vShaderFile.close();
		fShaderFile.close();

		vertexCode = vShaderStream.str();
		fragmentCode = fShaderStream.str();
	}
	catch (ifstream::failure e)
	{
		cout<<"ERROR:FILE_NOT_SUCCESFULLY_READ"<<endl;
	}

	//获取字符串后要将其转换为const char *
	const char* vShaderCode = vertexCode.c_str();
	const char* fShaderCode = fragmentCode.c_str();

	GLuint vertexShader = glCreateShader(GL_VERTEX_SHADER);
	GLuint fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);

	glShaderSource(vertexShader,1,&vShaderCode,NULL);
	glShaderSource(fragmentShader, 1, &fShaderCode, NULL);
	glCompileShader(vertexShader);
	glCompileShader(fragmentShader);

	int success;
	char infoLog[512];
	glGetShaderiv(vertexShader,GL_COMPILE_STATUS,&success);
	if (!success) {
		glGetShaderInfoLog(vertexShader,512,NULL,infoLog);
		cout << "ERROR:VERTEX SHADER FAILED COMPILED："<<infoLog << endl;
	}

	glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
	if (!success) {
		glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
		cout << "ERROR:FRAGMENT SHADER FAILED COMPILED："<<infoLog << endl;
	}

	ID = glCreateProgram();
	glAttachShader(ID,vertexShader);
	glAttachShader(ID, fragmentShader);
	glDeleteShader(vertexShader);
	glDeleteShader(fragmentShader);
	glLinkProgram(ID);

	glGetProgramiv(ID,GL_LINK_STATUS,&success);
	if (!success) {
		glGetProgramInfoLog(ID,512,NULL,infoLog);
		cout << "ERROR:SHADER PROGRAM LINK FAILED：" << infoLog << endl;
	}

}

void Shader::Use(){
	glUseProgram(ID);
}

void Shader::SetBool(const string& name, bool value) const{
	glUniform1i(glGetUniformLocation(ID,name.c_str()),(int)value);
}

void Shader::SetInt(const string& name, int value) const{
	glUniform1i(glGetUniformLocation(ID, name.c_str()), value);
}

void Shader::SetFloat(const string& name, float value) const{
	glUniform1f(glGetUniformLocation(ID, name.c_str()), value);
}

void  Shader::SetVec3(const string& name, const glm::vec3 &value) const {
	glUniform3fv(glGetUniformLocation(ID,name.c_str()),1,&value[0]);
}

void  Shader::SetVec3(const string& name, float x, float y, float z) const {
	glUniform3f(glGetUniformLocation(ID, name.c_str()), x,y,z);
}

void  Shader::SetVec4(const string& name, const glm::vec4 &value) const {
	glUniform4fv(glGetUniformLocation(ID, name.c_str()), 1, &value[0]);
}

void  Shader::SetVec4(const string& name, float x, float y, float z, float w) const {
	glUniform4f(glGetUniformLocation(ID, name.c_str()), x, y, z, w);
}

void Shader::SetMat4(const string& name, const glm::mat4 &mat) const {
	glUniformMatrix4fv(glGetUniformLocation(ID, name.c_str()),1,GL_FALSE,&mat[0][0]);
}






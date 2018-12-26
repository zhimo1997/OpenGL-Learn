#version 450 core
struct Material{
	sampler2D diffuse;
	sampler2D specular;
	float shininess;
};

struct DirectionLight{
	vec3 direction;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct PointLight{
	vec3 position;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;
};

struct SpotLight{
	vec3 position;
	vec3 direction;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;

	float InnerCutOff;
	float OuterCutOff;
};

in vec2 Texcoord;
in vec3 fragWorldPos;
in vec3	Normal;

out vec4 FragColor;

uniform Material mat;
uniform DirectionLight directionLight;
uniform PointLight pointLight[4];
uniform SpotLight spotLight;
uniform vec3 viewPos;

//phong模型需要是三个方向向量
vec3 CalculateDirectionLight(DirectionLight light,vec3 normal,vec3 viewDir);
vec3 CalculatePointLight(PointLight light,vec3 normal,vec3 fragWorldPos,vec3 viewDir);
vec3 CalculateSpotLight(SpotLight light,vec3 normal,vec3 fragWorldPos,vec3 viewDir);

void main()
{
	vec3 normal=normalize(Normal);
	vec3 viewDir=normalize(viewPos-fragWorldPos);
	//方向光计算
	vec3 result=CalculateDirectionLight(directionLight,normal,viewDir);
	//点光源计算
	for(int i=0;i<4;i++){
		result+=CalculatePointLight(pointLight[i],normal,fragWorldPos,viewDir);
	}
	result+=CalculateSpotLight(spotLight,normal,fragWorldPos,viewDir);

	FragColor=vec4(result,1.0);
	
}

vec3 CalculateDirectionLight(DirectionLight light,vec3 normal,vec3 viewDir){
	vec3 ambientColor=light.ambient*vec3(texture(mat.diffuse,Texcoord));
	vec3 lightDir=normalize(-light.direction);
	float diff=max(0,dot(normal,lightDir));
	vec3 diffuseColor=light.diffuse*(vec3(texture(mat.diffuse,Texcoord))*diff);

	vec3 reflectDir=reflect(-lightDir,normal);
	float spec=pow(max(dot(viewDir,reflectDir),0),mat.shininess);
	vec3 specularColor=light.specular*(vec3(texture(mat.specular,Texcoord))*spec);

	return (ambientColor+diffuseColor+specularColor);
}

//点光源要考虑到光线的衰减，但不考虑环境光
vec3 CalculatePointLight(PointLight light,vec3 normal,vec3 fragWorldPos,vec3 viewDir){
	vec3 ambientColor=light.ambient*vec3(texture(mat.diffuse,Texcoord));
	//计算光线衰减程度
	float distance=length(light.position-fragWorldPos);
	float attenuation=1.0/(light.constant+light.linear*distance+light.quadratic*(distance*distance));

	vec3 lightDir=normalize(light.position-fragWorldPos);

	float diff=max(0,dot(normal,lightDir));
	vec3 diffuseColor=light.diffuse*(vec3(texture(mat.diffuse,Texcoord))*diff);
	diffuseColor*=attenuation;

	vec3 reflectDir=reflect(-lightDir,normal);
	float spec=pow(max(dot(viewDir,reflectDir),0),mat.shininess);
	vec3 specularColor=light.specular*(vec3(texture(mat.specular,Texcoord))*spec);
	specularColor*=attenuation;

	return ambientColor+diffuseColor+specularColor;
}

//聚光源要注意平滑边缘，不考虑环境光
vec3 CalculateSpotLight(SpotLight light,vec3 normal,vec3 fragWorldPos,vec3 viewDir){
	vec3 lightDir=normalize(light.position-fragWorldPos);

	float distance=length(light.position-fragWorldPos);
	float attenuation=1.0/(light.constant+light.linear*distance+light.quadratic*(distance*distance));

	float theta=dot(lightDir,normalize(-light.direction));
	float epsilon=light.InnerCutOff-light.OuterCutOff;
	float intensity=clamp((theta-light.OuterCutOff)/epsilon,0,1);
	vec3 ambientColor=light.ambient*vec3(texture(mat.diffuse,Texcoord));

	float diff=max(0,dot(normal,lightDir));
	vec3 diffuseColor=light.diffuse*(vec3(texture(mat.diffuse,Texcoord))*diff);
	diffuseColor*=intensity*attenuation;

	vec3 reflectDir=reflect(-lightDir,normal);
	float spec=pow(max(dot(viewDir,reflectDir),0),mat.shininess);
	vec3 specularColor=light.specular*(vec3(texture(mat.specular,Texcoord))*spec);
	specularColor*=intensity*attenuation;

	return ambientColor+diffuseColor+specularColor;
}

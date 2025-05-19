#version 330 core
in vec2 texCoord; // Input from vertex shader
out vec4 FragColor;
uniform sampler2D textureData;
uniform vec3 spriteColor;
void main()
{
    vec4 texColor = texture(textureData, texCoord);
    FragColor = texColor * vec4(spriteColor, 1.0);
}
#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord; // Changed to vec2 to match data
uniform mat4 projection;
uniform mat4 model;
out vec2 texCoord; // Changed to vec2
void main()
{
    gl_Position = projection * model * vec4(aPosition, 1.0);
    texCoord = aTexCoord; // Pass to fragment shader
}
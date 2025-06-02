#version 330 core
in vec2 texCoord; // Input from vertex shader
out vec4 FragColor;
uniform sampler2D textureData;
uniform vec3 spriteColor;
uniform float frameOffsetX; 
uniform float frameOffsetY;  
uniform float frameScaleX;   
uniform float frameScaleY;   
void main()
{
   vec2 actualUV = (texCoord * vec2(frameScaleX, frameScaleY)) + vec2(frameOffsetX, frameOffsetY);
    
    vec4 texColor = texture(textureData, actualUV);
    FragColor = vec4(texColor.rgb * spriteColor, texColor.a);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Rendering;
using GameEngine.Resources;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.ECS.Components
{
    public class SpriteRenderer : Component
    {
        public Texture Texture { get; set; }
        public Color4 Color { get; set; }

        private Transform transform;
        public int CurrentFrame { get; set; } = 0;
      
        public int GridWidth { get; set; } = 1;       
        public int GridHeight { get; set; } = 1;    

      

        public override void Initialize()
        {
            transform = Entity.GetComponent<Transform>();

            if(transform == null)
            {
                transform = Entity.AddComponent<Transform>();
            }

        }

        public override void Render()
        {
            if (Texture == null) return;

           
            Shader shader = ContentManager.Main.GetSpriteShader();

            if (shader == null) return;

            shader.Use();
            if (Color.R == 0 && Color.G == 0 && Color.B == 0 && Color.A == 0)
            {
                Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f); // Set white as default
               
            }

            Matrix4 modelMatrix = transform.GetTransformationMatrix();
            shader.SetMatrix4("model", modelMatrix);

            Texture.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);

            shader.SetInt("textureData",0);
           

            shader.SetVector3("spriteColor", new Vector3(Color.R, Color.G, Color.B));

            CalculateAndSetFrameUniforms(shader);


            GL.BindVertexArray(ContentManager.Main.GetQuadVAO());

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);

           

        }
        private void CalculateAndSetFrameUniforms(Shader shader)
        {
            // Convert frame number to grid coordinates
            int frameX = CurrentFrame % GridWidth;   // Column (0 or 1 for 2x2)
            int frameY = CurrentFrame / GridWidth;   // Row (0 or 1 for 2x2)

            // Calculate frame size and offset
            float frameScaleX = 1.0f / GridWidth;    // 0.5 for 2x2
            float frameScaleY = 1.0f / GridHeight;   // 0.5 for 2x2
            float frameOffsetX = frameX * frameScaleX; // 0.0 or 0.5
            float frameOffsetY = frameY * frameScaleY; // 0.0 or 0.5

            // Send to shader
            shader.SetFloat("frameOffsetX", frameOffsetX);
            shader.SetFloat("frameOffsetY", frameOffsetY);
            shader.SetFloat("frameScaleX", frameScaleX);
            shader.SetFloat("frameScaleY", frameScaleY);
        }


    }
}

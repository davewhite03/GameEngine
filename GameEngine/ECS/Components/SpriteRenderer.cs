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
                Console.WriteLine("Setting default color to white");
            }

            Matrix4 modelMatrix = transform.GetTransformationMatrix();
            shader.SetMatrix4("model", modelMatrix);

            Texture.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);

            shader.SetInt("textureData",0);
           

            shader.SetVector3("spriteColor", new Vector3(Color.R, Color.G, Color.B));

            
            GL.BindVertexArray(ContentManager.Main.GetQuadVAO());

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);

           

        }

        
    }
}

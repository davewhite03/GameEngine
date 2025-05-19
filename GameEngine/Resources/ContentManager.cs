using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Rendering;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Resources
{
    public class ContentManager : IDisposable
    {
        private static ContentManager mainInstance;
        private Dictionary<string, Texture> Texture = new Dictionary<string, Texture>();
        private Dictionary<string, Shader> Shader = new Dictionary<string, Shader>();
        private Dictionary<string, int> Meshes = new Dictionary<string, int>();

        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;


        public static ContentManager Main
        {
            get { return mainInstance; }
        }

        public void SetAsMain()
        {
            mainInstance = this;
        }

        public void InitializeEngineResources()
        {
            CreateQuadMesh();
        }

        private void CreateQuadMesh()
        {
            float[] vertices = {
                
                 0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 
                 0.5f, -0.5f, 0.0f,   1.0f, 1.0f, 
                -0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 
                -0.5f,  0.5f, 0.0f,   0.0f, 0.0f  
            };

            uint[] indices = {
                0, 1, 3, 
                1, 2, 3   
            };
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);
            Meshes["quad"] = VertexArrayObject;

        }

        public int GetQuadVAO()
        {
            return GetMesh("quad");
        }
        public Texture LoadTexture(string name, string path)
        {
            if(Texture.TryGetValue(name, out Texture texture))
                return texture;

            texture = new Texture(path);
            Texture[name] = texture;

            return texture;
        }

        public int GetMesh(string name)
        {
            if (Meshes.TryGetValue(name, out int mesh))
                return mesh;
            return 0;
        }
        public Texture GetTexture(string name)
        {
            if(Texture.TryGetValue(name, out Texture texture))
                return texture;
            return null;
        }

        public Shader GetShader(string name)
        {
            if (Shader.TryGetValue(name, out Shader shader)) 
                return shader;

            return null;
        }
        public Shader LoadShader(string name, string vertPath, string fragPath)
        {
            if (Shader.TryGetValue(name, out Shader shader))
                return shader;

            shader = new Shader(vertPath, fragPath);
            Shader[name] = shader;
            return shader;

        }

        public Shader GetSpriteShader()
        {
            return GetShader("sprite");
        }
        public void Unload()
        {

        }

        public void Dispose()
        {
            foreach(Texture texture in Texture.Values)
            {
                texture.Dispose();
            }

            foreach(Shader shader in Shader.Values)
            {
                shader.Dispose();
            }
            foreach(int vao in Meshes.Values)
            {
                GL.DeleteVertexArray(vao);
            }
            Texture.Clear();
            Shader.Clear();
            Meshes.Clear();
        }
    }
}

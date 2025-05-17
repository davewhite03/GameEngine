using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine
{
    public class Game : GameWindow
    {
        Shader shader;
        Matrix4 projectionMatrix;
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            

            

        }
        protected override void OnLoad()
        {
           
            base.OnLoad();
            
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            shader = new Shader("C:\\GameEngine\\GameEngine\\Shader.vert", "C:\\GameEngine\\GameEngine\\Shader.frag");
            VertexArrayObject = GL.GenVertexArray();
            
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(1);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            // ..:: Initialization code (done once (unless your object frequently changes)) :: ..

// 2. copy our vertices array in a buffer for OpenGL to use

GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            UpdateProjectionMatrix(ClientSize.X, ClientSize.Y);

        }
         protected override void OnUnload()
        {
            shader.Dispose();

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

           
           

          
            shader.Use();

         
            GL.BindVertexArray(VertexArrayObject);

            
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

    

            SwapBuffers();
        }
       
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            UpdateProjectionMatrix(e.Width, e.Height);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                
            }

        }

        protected void UpdateProjectionMatrix(int width, int height)
        {
            

            float aspectRatio = width / (float)height;
            projectionMatrix = Matrix4.CreateOrthographicOffCenter(
                -aspectRatio, aspectRatio,
                -1.0f, 1.0f,
                 -1.0f,1.0f
                );

            shader.SetMatrix4("projection",projectionMatrix);
            
        }


        float[] vertices = 
        {
    0.5f,  0.5f, 0.0f,  1.0f, 0.0f, 1.0f,  // top right - red
     0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,  // bottom right - green
    -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  // bottom left - blue
    -0.5f,  0.5f, 0.0f,  1.0f, 1.0f, 0.0f
        };

        uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };
     
    }
    }

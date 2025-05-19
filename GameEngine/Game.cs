using GameEngine.ECS;
using GameEngine.ECS.Components;
using GameEngine.Rendering;
using GameEngine.Resources;
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
        public Shader MainShader;
        Matrix4 projectionMatrix;
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        private Scene currentScene;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            
        }

        protected override void OnLoad()
        {
           
            base.OnLoad();
            
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            MainShader = new Shader("C:\\GameEngine\\GameEngine\\Shader.vert", "C:\\GameEngine\\GameEngine\\Shader.frag");
           
           ContentManager contentManager = new ContentManager();
            contentManager.SetAsMain();
            
            contentManager.InitializeEngineResources();

            contentManager.LoadShader("sprite", "C:\\GameEngine\\GameEngine\\Shader.vert", "C:\\GameEngine\\GameEngine\\Shader.frag");

            UpdateProjectionMatrix(ClientSize.X, ClientSize.Y);

            currentScene = new Scene();

            Entity player = currentScene.CreateEntity("Player");


            Transform playerTransform = player.AddComponent<Transform>();
            playerTransform.Position = new Vector2(0,0);

            SpriteRenderer playerSprite = player.AddComponent<SpriteRenderer>();

            
            playerSprite.Texture = contentManager.LoadTexture("player", "C:\\GameEngine\\GameEngine\\Assets\\Player.png");

            RigidBody rigidBody = player.AddComponent<RigidBody>();
            rigidBody.Mass = 1.0f;

            BoxCollider playerCollider = player.AddComponent<BoxCollider>();
            playerCollider.Size = new Vector2(1.0f, 1.0f);

        }

         protected override void OnUnload()
        {
           if(ContentManager.Main != null)
            {
                ContentManager.Main.Dispose();
            }

            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);


            GL.Clear(ClearBufferMask.ColorBufferBit);


            MainShader.Use();

            MainShader.SetMatrix4("projection", projectionMatrix);

            currentScene.Render();

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

            float deltaTime = (float)e.Time;
            HandlerPlayerInput(deltaTime);
            currentScene.Update(deltaTime);
           
        }

        public void HandlerPlayerInput(float deltaTime)
        {
            Entity playerEntity = currentScene.FindEntityByName("Player");
            if (playerEntity != null)
            {
                Console.WriteLine("player entity found!");
                RigidBody rb = playerEntity.GetComponent<RigidBody>();
                if (rb != null)
                {
                    Vector2 force = Vector2.Zero;

                    if (KeyboardState.IsKeyDown(Keys.W))
                        force.Y = .01f;                      
                    if (KeyboardState.IsKeyDown(Keys.S))
                        force.Y = -.01f;
                    if (KeyboardState.IsKeyDown(Keys.A))
                        force.X = -.01f;
                    if (KeyboardState.IsKeyDown(Keys.D))
                        force.X = .01f;

                    rb.ApplyForce(force);
                }
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

            MainShader.SetMatrix4("projection",projectionMatrix);

            Shader spriteShader = ContentManager.Main.GetSpriteShader();
            if (spriteShader != null)
            {
                spriteShader.Use();
                spriteShader.SetMatrix4("projection", projectionMatrix);
            }

        }

        public Shader GetShader()
        {
            return MainShader;
        }

     
        public int GetSpriteVAO()
        {
            return VertexArrayObject;
        }

    }
}

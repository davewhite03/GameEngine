using GameEngine.ECS;
using GameEngine.ECS.Components;
using GameEngine.ECS.System;
using GameEngine.ECS.Systems;
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
        SystemManager systemManager;
        CameraSystem cameraSystem;
        Entity cameraEntity;     
        InputSystem inputSystem;
        MovementSystem movementSystem;
        PhysicsSystem physicsSystem;
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

           

            currentScene = new Scene();
            Entity platform = currentScene.CreateEntity("Platform");


            Transform platformTransform = platform.AddComponent<Transform>();
            platformTransform.Scale = new Vector2(1.0f, 1.0f);
            platformTransform.Position = new Vector2(0, -1);

            SpriteRenderer platformSprite = platform.AddComponent<SpriteRenderer>();
            

            platformSprite.Texture = contentManager.LoadTexture("platform", "C:\\GameEngine\\GameEngine\\Assets\\platform.png");

            RigidBody platformRigidBody = platform.AddComponent<RigidBody>();
            platformRigidBody.Mass = 1.0f;

            BoxCollider platformCollider = platform.AddComponent<BoxCollider>();
            platformCollider.Size = new Vector2(1.0f, 1.0f);


            Entity platform2 = currentScene.CreateEntity("Platform2");


            Transform platformTransform2 = platform2.AddComponent<Transform>();
            platformTransform2.Scale = new Vector2(1.0f, 1.0f);
            platformTransform2.Position = new Vector2(-3, -1);

            SpriteRenderer platformSprite2 = platform2.AddComponent<SpriteRenderer>();


            platformSprite2.Texture = contentManager.LoadTexture("platform", "C:\\GameEngine\\GameEngine\\Assets\\platform.png");

            RigidBody platformRigidBody2 = platform2.AddComponent<RigidBody>();
            platformRigidBody.Mass = 1.0f;

            BoxCollider platformCollider2 = platform2.AddComponent<BoxCollider>();
            platformCollider.Size = new Vector2(1.0f, .5f);
            
            Entity player = currentScene.CreateEntity("Player");


            Transform playerTransform = player.AddComponent<Transform>();
            playerTransform.Position = new Vector2(0,0);
            

            SpriteRenderer playerSprite = player.AddComponent<SpriteRenderer>();

            
            playerSprite.Texture = contentManager.LoadTexture("player", "C:\\GameEngine\\GameEngine\\Assets\\Jumper.png");

            RigidBody rigidBody = player.AddComponent<RigidBody>();
            rigidBody.Mass = 1.0f;

            BoxCollider playerCollider = player.AddComponent<BoxCollider>();
            playerCollider.Size = new Vector2(1.0f, 1.0f);
            
            PlayerMovement playerMovement = player.AddComponent<PlayerMovement>();
            playerMovement.MoveSpeed = 1.0f;
            playerMovement.JumpForce = 2.05f;


            cameraEntity = currentScene.CreateEntity("MainCamera");
            Camera mainCamera = cameraEntity.AddComponent<Camera>();
            mainCamera.Target = player;
            mainCamera.FollowSpeed = 3.0f;
            mainCamera.SmoothFollow = true;


            /*
           Entity enemy = currentScene.CreateEntity("Enemy");


            Transform enemyTransform = enemy.AddComponent<Transform>();
            enemyTransform.Position = new Vector2(0, -1);

            SpriteRenderer enemySprite = enemy.AddComponent<SpriteRenderer>();


            enemySprite.Texture = contentManager.LoadTexture("enemy", "C:\\GameEngine\\GameEngine\\Assets\\Player (3).png");

            RigidBody enemyRigidBody = enemy.AddComponent<RigidBody>();
            enemyRigidBody.Mass = 1.0f;

            BoxCollider enemyCollider = enemy.AddComponent<BoxCollider>();
            enemyCollider.Size = new Vector2(0.5f, 0.5f);
*/
            inputSystem = new InputSystem(currentScene, KeyboardState);
            movementSystem = new MovementSystem(currentScene);
            systemManager = new SystemManager();
            physicsSystem = new PhysicsSystem(currentScene);
            cameraSystem = new CameraSystem(currentScene);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

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
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            float deltaTime = (float)e.Time;
            if(!CollisionDetection()) Gravity();

            
            inputSystem.Update(deltaTime);
            movementSystem.Update(deltaTime);
           
            currentScene.Update(deltaTime);
            cameraSystem.Update(deltaTime);
            UpdateProjectionMatrixWithCamera();
           
        }
        public void Gravity()
        {
            Entity playerEntity = currentScene.FindEntityByName("Player");
            if (playerEntity != null)
            {

                RigidBody rb = playerEntity.GetComponent<RigidBody>();
                if (rb != null)
                {

                    Vector2 force = Vector2.Zero;
                    force.Y = -.001f;
                    rb.ApplyForce(force);

                }
            }
        }
        public bool CollisionDetection()
        {
            List<Entity> entities = currentScene.GetEntitiesWith();

            Entity playerEntity = currentScene.FindEntityByName("Player");
            BoxCollider playerBoxCollider = playerEntity.GetComponent<BoxCollider>();
            PlayerMovement playerMovement = playerEntity.GetComponent<PlayerMovement>();

            playerMovement.IsGrounded = false;

            foreach (Entity entity in entities)
            {
                if (entity.Name == "Player") continue;

                var otherCollider = entity.GetComponent<BoxCollider>();

                if (otherCollider != null && playerBoxCollider.Intersects(otherCollider))
                {
                    var playerPosition = playerEntity.GetComponent<Transform>();
                    var otherPosition = entity.GetComponent<Transform>();

                    if(playerPosition.Position.Y > otherPosition.Position.Y)
                    {
                        playerMovement.IsGrounded = true;

                    }
                    playerEntity.GetComponent<RigidBody>().Stop();

                    return true;
                } 
            }
            return false;


        }
       

        protected void UpdateProjectionMatrixWithCamera()
        {
            Camera mainCamera = cameraEntity?.GetComponent<Camera>();
            if (mainCamera == null) return;


            float aspectRatio = ClientSize.X / (float)ClientSize.Y;

            float viewWidth = aspectRatio / mainCamera.Zoom;
            float viewHeight = 1.0f / mainCamera.Zoom;

            projectionMatrix = Matrix4.CreateOrthographicOffCenter(
                mainCamera.Position.X - viewWidth,
                mainCamera.Position.X + viewWidth,
                mainCamera.Position.Y - viewHeight,
                mainCamera.Position.Y + viewHeight,
                 -1.0f, 1.0f
                );

            MainShader.Use();
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

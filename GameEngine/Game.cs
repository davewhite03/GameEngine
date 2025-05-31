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
        CollisionSystem collisionSystem;    
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
            MainShader = new Shader("Shader.vert", "Shader.frag");
           
           ContentManager contentManager = new ContentManager();
            contentManager.SetAsMain();
            
            contentManager.InitializeEngineResources();

            contentManager.LoadShader("sprite", "Shader.vert", "Shader.frag");

           

            currentScene = new Scene();
            
            Entity platform = currentScene.CreateEntity("Platform");


            Transform platformTransform = platform.AddComponent<Transform>();
            platformTransform.Scale = new Vector2(1.0f, 1.0f);
            platformTransform.Position = new Vector2(1, 0);

            SpriteRenderer platformSprite = platform.AddComponent<SpriteRenderer>();
            

            platformSprite.Texture = contentManager.LoadTexture("platform", "Assets\\platform.png");

            RigidBody platformRigidBody = platform.AddComponent<RigidBody>();
            platformRigidBody.Mass = 1.0f;
            platformRigidBody.IsKinematic = true;

            BoxCollider platformCollider = platform.AddComponent<BoxCollider>();
            platformCollider.Size = new Vector2(1.0f, 1.0f);

            
            Entity platform2 = currentScene.CreateEntity("Platform2");


            Transform platformTransform2 = platform2.AddComponent<Transform>();
            platformTransform2.Scale = new Vector2(8.0f, 1.0f);
            platformTransform2.Position = new Vector2(4, -1);

            SpriteRenderer platformSprite2 = platform2.AddComponent<SpriteRenderer>();


            platformSprite2.Texture = contentManager.LoadTexture("platform", "Assets\\platform.png");

            RigidBody platformRigidBody2 = platform2.AddComponent<RigidBody>();
            platformRigidBody2.Mass = 1.0f;
           

            BoxCollider platformCollider2 = platform2.AddComponent<BoxCollider>();
            platformCollider2.Size = new Vector2(1.0f, 1.0f);
            
            
            Entity player = currentScene.CreateEntity("Player");


            Transform playerTransform = player.AddComponent<Transform>();
            playerTransform.Position = new Vector2(0,0);

            PlayerInput playerInput = player.AddComponent<PlayerInput>();


            SpriteRenderer playerSprite = player.AddComponent<SpriteRenderer>();

            
            playerSprite.Texture = contentManager.LoadTexture("player", "Assets\\Jumper.png");

            RigidBody rigidBody = player.AddComponent<RigidBody>();
            rigidBody.Mass = 1.0f;

            BoxCollider playerCollider = player.AddComponent<BoxCollider>();
            playerCollider.Size = new Vector2(1.0f, .09f);
            
            PlayerMovement playerMovement = player.AddComponent<PlayerMovement>();
            playerMovement.MoveSpeed = 1.0f;
            playerMovement.JumpForce = 2.5f;


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
            collisionSystem = new CollisionSystem(currentScene);
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

           
            
            inputSystem.Update(deltaTime);
            
            movementSystem.Update(deltaTime);
            physicsSystem.Update(deltaTime);

            collisionSystem.Update(deltaTime);
            currentScene.Update(deltaTime);
            
            cameraSystem.Update(deltaTime);
           
            UpdateProjectionMatrixWithCamera();
           
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

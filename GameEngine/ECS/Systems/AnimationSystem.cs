using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.ECS.Components;
using GameEngine.Rendering;
using GameEngine.Resources;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.ECS.Systems
{
    public class AnimationSystem : GameSystem
    {
        private float frameTimer = 0f;
        private float frameTime = 0.2f;  // Time per frame (0.2 = 5 frames per second)
        private string facingDirection = "right";
        // NEW: Track current animation state
        private string currentAnimation = "";
        private bool animationComplete = false;

        ContentManager _contentManager;
        public AnimationSystem(Scene scene, ContentManager contentManager) : base(scene) 
        {
            _contentManager = contentManager;
        }

    



        public override void Update(float deltaTime)
        {
            frameTimer += deltaTime;

            if (frameTimer >= frameTime)
            {
                frameTimer = 0f;
                UpdateAnimationFrames(deltaTime);
            }
        }

        private void UpdateAnimationFrames(float deltaTime)
        {
            List<Entity> list = scene.GetEntitiesWith<PlayerInput>().ToList();
            foreach (Entity entity in list)
            {
                var playerInput = entity.GetComponent<PlayerInput>();
                var playerSprite = entity.GetComponent<SpriteRenderer>();

                if (playerInput.Moves.Count != 0)
                {
                    var lastMove = playerInput.Moves.Last();

                    if (lastMove.Equals("D"))
                    {
                        facingDirection = "right"; // Update facing direction
                        StartAnimation("walkRight", playerSprite);
                        AdvanceLoopingAnimation(playerSprite, 4);
                    }
                    else if (lastMove.Equals("A"))
                    {
                        facingDirection = "left"; // Update facing direction
                        StartAnimation("walkLeft", playerSprite);
                        AdvanceLoopingAnimation(playerSprite, 4);
                    }
                    else if (lastMove.Equals("W"))
                    {
                        // Use current facing direction for jump
                        string jumpAnimation = facingDirection == "left" ? "jumpLeft" : "jumpRight";
                        StartAnimation(jumpAnimation, playerSprite);
                        AdvanceOneShotAnimation(playerSprite, 4);
                    }
                }
            }
        }
        // Helper method to cycle through frames
        private void StartAnimation(string animationName, SpriteRenderer sprite)
        {
            if (currentAnimation != animationName)
            {
                currentAnimation = animationName;
                animationComplete = false;
                sprite.CurrentFrame = 0;

                switch (animationName)
                {
                    case "walkRight":
                        sprite.Texture = _contentManager.LoadTexture("playerWalkingright", "Assets\\Main Guy Walk.png");
                        break;
                    case "walkLeft":
                        sprite.Texture = _contentManager.LoadTexture("playerwalkingleft", "Assets\\Main Guy Walk Right.png");
                        break;
                    case "jumpLeft":
                        sprite.Texture = _contentManager.LoadTexture("playerjumpleft", "Assets\\Main Guy Jump Left.png");
                        break;
                    case "jumpRight":
                        sprite.Texture = _contentManager.LoadTexture("playerjumpright", "Assets\\Main Guy Jump Right.png");
                        break;
                }

                sprite.GridWidth = 2;
                sprite.GridHeight = 2;
            }
        }

        private void AdvanceLoopingAnimation(SpriteRenderer sprite, int frameCount)
        {
            sprite.CurrentFrame = (sprite.CurrentFrame + 1) % frameCount; // Loop 0-3
        }

        private void AdvanceOneShotAnimation(SpriteRenderer sprite, int frameCount)
        {
            if (!animationComplete)
            {
                sprite.CurrentFrame++;

                if (sprite.CurrentFrame >= frameCount)
                {
                    sprite.CurrentFrame = frameCount - 1; // Stay on last frame
                    animationComplete = true;
                    Console.WriteLine("Jump animation complete!");
                }
            }
            // If animation is complete, don't advance frames anymore
        }


    }
}
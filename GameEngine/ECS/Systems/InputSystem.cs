using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.ECS.Components;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.ECS.System
{
    public class InputSystem : GameSystem
    {
        KeyboardState _keyboardState;
        


        public InputSystem(Scene scene , KeyboardState keyboardState) : base(scene)
        {
           this. _keyboardState = keyboardState;
        }

        public override void Update(float deltaTime)
        {
            var inputEntities = scene.GetEntitiesWith<PlayerInput>();

            foreach (var entity in inputEntities)
            {
                var input = entity.GetComponent<PlayerInput>();
                
                
                Vector2 moveInput = Vector2.Zero;

                if(_keyboardState.IsKeyDown(Keys.A)) moveInput.X = -1.0f;

                if(_keyboardState.IsKeyDown(Keys.D)) moveInput.X = 1.0f;

                Console.WriteLine(moveInput.X);

              
                input.MoveInput = moveInput;
                input.JumpPressed = _keyboardState.IsKeyPressed(Keys.W);


            }
        }
    }
}

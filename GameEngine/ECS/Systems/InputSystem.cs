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
                
                var rb = entity.GetComponent<RigidBody>();
                Vector2 force = Vector2.Zero;

                if(_keyboardState.IsKeyPressed(Keys.A)) force.X = 1;

                if(_keyboardState.IsKeyPressed(Keys.D)) force.X = -1;
                if (_keyboardState.IsKeyReleased(Keys.D)) force.X = 1;

                if (_keyboardState.IsKeyReleased(Keys.A)) force.X = -1;

                Console.WriteLine(force.X);


                rb.ApplyForce(force);   

                input.JumpPressed = _keyboardState.IsKeyPressed(Keys.W);


            }
        }
    }
}

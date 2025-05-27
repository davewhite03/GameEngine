using GameEngine.ECS.Components;
using OpenTK.Mathematics;

namespace GameEngine.ECS.Systems
{
    public class MovementSystem : GameSystem
    {
        public MovementSystem(Scene scene) : base(scene) { }

        public override void Update(float deltaTime)
        {

            var movableEntities = scene.GetEntitiesWith<PlayerInput, PlayerMovement, RigidBody>();

            foreach (var entity in movableEntities) 
            {

                var input = entity.AddComponent<PlayerInput>();
                var movement = entity.AddComponent<PlayerMovement>();
                var rigidBody = entity.AddComponent<RigidBody>();

                Vector2 force = Vector2.Zero;

                force.X = input.MoveInput.X * movement.MoveSpeed * deltaTime;


                if(input.JumpPressed && movement.IsGrounded)
                {
                    force.Y = movement.JumpForce;
                    movement.IsGrounded = false;
                }

                rigidBody.ApplyForce(force);
            
            }

        }
    }
}

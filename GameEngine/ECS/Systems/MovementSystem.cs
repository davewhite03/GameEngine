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
                
                var input = entity.GetComponent<PlayerInput>();
                var movement = entity.GetComponent<PlayerMovement>();
                var rigidBody = entity.GetComponent<RigidBody>();
                

                Vector2 force = Vector2.Zero;
                var targetVelocity = rigidBody.Velocity;
                targetVelocity.X = input.MoveInput.X * movement.MoveSpeed ;
               

                if (input.JumpPressed && movement.IsGrounded)
                {
                    targetVelocity.Y = movement.JumpForce;
                    movement.IsGrounded = false;
                }

               rigidBody.Velocity = targetVelocity;

                
            }
        }
    }
}

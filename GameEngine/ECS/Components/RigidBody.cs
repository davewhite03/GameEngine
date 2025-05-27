using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace GameEngine.ECS.Components
{
    public class RigidBody : Component
    {
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public float Mass { get; set; } = 1.0f;
        public bool IsKinematic { get; set; } = false;

        private Transform transform;

        public override void Initialize()
        {
            transform = Entity.GetComponent<Transform>();

            if(transform == null)
            {
                transform = Entity.AddComponent<Transform>();
            }
        }

        public override void Update(float deltaTime)
        {
            if (IsKinematic) return;

            transform.Position += Velocity * deltaTime;
        }

        public void  ApplyForce(Vector2 force)
        {
            if (IsKinematic) return;

            Velocity += force / Mass;
        }

        public void Stop()
        {
            Velocity -= Velocity;
        }
    }
}

 using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using GameEngine.ECS.Components;
using OpenTK.Mathematics;

namespace GameEngine.ECS.Systems
{
    public class CollisionSystem : GameSystem
    {
        public CollisionSystem(Scene scene) : base(scene) { }

        public override void Update(float deltaTime)
        {
            var collidableEntities = scene.GetEntitiesWith<BoxCollider>();

            for (int i = 0; i < collidableEntities.Count; i++)
            {
                for (int j = i+1; j < collidableEntities.Count; j++)
                {
                    CheckForCollision(collidableEntities[i], collidableEntities[j]);
                }
            }
        }

        public void CheckForCollision(Entity entityA, Entity entityB)
        {
            BoxCollider boxColliderA = entityA.GetComponent<BoxCollider>();
            BoxCollider boxColliderB = entityB.GetComponent<BoxCollider>();

          
            if (entityA.GetComponent<PlayerMovement>() != null)
            {
                entityA.GetComponent<PlayerMovement>().IsGrounded = false;

            }
            if (entityB.GetComponent<PlayerMovement>() != null)
            {

                entityB.GetComponent<PlayerMovement>().IsGrounded = false;
            }
            if (boxColliderA.Intersects(boxColliderB))
            {
                HandleCollision(entityA, entityB, boxColliderA, boxColliderB);
            }
        }

        public void HandleCollision(Entity entityA, Entity entityB, BoxCollider boxColliderA, BoxCollider boxColliderB)
        {
            RigidBody rigidBodyA = entityA.GetComponent<RigidBody>();
            RigidBody rigidBodyB = entityB.GetComponent<RigidBody>();

            if (rigidBodyA != null && rigidBodyB != null)
            {
                ResolvePyshicsCollision(entityA, entityB, boxColliderA, boxColliderB, rigidBodyA, rigidBodyB );
            }
        }

        public void ResolvePyshicsCollision(Entity entityA, Entity entityB, BoxCollider boxColliderA, BoxCollider boxColliderB, RigidBody rigidBodyA, RigidBody rigidBodyB) 
        {
            if (rigidBodyA.IsKinematic && rigidBodyB.IsKinematic) return;

            var boundsA = boxColliderA.GetBounds();
            var boundsB = boxColliderB.GetBounds();

            var overlapX = Math.Min(boundsA.Right - boundsB.Left, boundsB.Right - boundsA.Left);
            var overlapY = Math.Min(boundsA.Bottom - boundsB.Top, boundsB.Bottom - boundsA.Top);

            if (overlapX < overlapY)
            {
                ResolveHorizontalCollision(entityA, entityB, rigidBodyA, rigidBodyB, overlapX, boundsA, boundsB);
            }
            else {
                ResolveVerticalCollision(entityA, entityB, rigidBodyA, rigidBodyB, overlapY, boundsA ,boundsB);
            
            }
            

        }

        public void ResolveVerticalCollision(Entity entityA, Entity entityB, RigidBody rbA, RigidBody rbB, float overLapY, RectangleF boundsA, RectangleF boundsB)
        {
            var transformA = entityA.GetComponent<Transform>();
            var transformB = entityB.GetComponent<Transform>();
         
            bool aOnTop = boundsA.Y > boundsB.Y;

            

            if (aOnTop)
            {
                transformA.Position = new Vector2(transformA.Position.X, transformA.Position.Y + overLapY / 2);
                transformB.Position = new Vector2(transformB.Position.X, transformB.Position.Y - overLapY / 2);

                if (rbA.Velocity.Y < 0) rbA.Velocity = new Vector2(rbA.Velocity.X, 0);

                var playerMovement = entityA.GetComponent<PlayerMovement>();
                if (playerMovement != null) 
                {
                    playerMovement.IsGrounded = true;
                    Console.WriteLine(playerMovement+"Player movement has been set");
                }  

            }
            else {
                transformA.Position = new Vector2(transformA.Position.X, transformA.Position.Y - overLapY / 2);
                transformB.Position = new Vector2(transformB.Position.X, transformB.Position.Y + overLapY / 2);
                if (rbB.Velocity.Y < 0) rbB.Velocity = new Vector2(rbB.Velocity.X, 0);

                var playerMovement = entityB.GetComponent<PlayerMovement>();
                if (playerMovement != null) playerMovement.IsGrounded = true;
                Console.WriteLine(playerMovement + "Player movement has been set");

            }

        }
        public void ResolveHorizontalCollision(Entity entityA, Entity entityB, RigidBody rbA, RigidBody rbB, float overLapX, RectangleF boundsA, RectangleF boundsB)
        {
            var transformA = entityA.GetComponent<Transform>();
            var transformB = entityB.GetComponent<Transform>();

            bool aOnTop = boundsA.X > boundsB.X;



            if (aOnTop)
            {
                transformA.Position = new Vector2(transformA.Position.X + overLapX / 2, transformA.Position.Y );
                transformB.Position = new Vector2(transformB.Position.X - overLapX / 2, transformB.Position.Y);

                if (rbA.Velocity.X < 0) rbA.Velocity = new Vector2(0, rbA.Velocity.Y);

              

            }
            else
            {
                transformA.Position = new Vector2(transformA.Position.X - overLapX / 2, transformA.Position.Y );
                transformB.Position = new Vector2(transformB.Position.X + overLapX / 2, transformB.Position.Y );
                if (rbB.Velocity.X < 0) rbB.Velocity = new Vector2(0, rbB.Velocity.Y);

               

            }

        }

    }
}

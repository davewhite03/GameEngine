
using GameEngine.ECS.Components;
using OpenTK.Mathematics;

namespace GameEngine.ECS.Systems
{
    public class PhysicsSystem
    {
        List<Entity> entities = new List<Entity>();
        Scene currentScene;


        public PhysicsSystem(Scene scene)
        {
            currentScene = scene;
        }


        public void Update()
        {
            foreach (Entity entity in entities)
            {
                var entityMovement = entity.GetComponent<PlayerMovement>();
                RigidBody rb = entity.GetComponent<RigidBody>();
                if (rb != null)
                {

                    Vector2 force = Vector2.Zero;
                    force.Y = -.001f;
                    rb.ApplyForce(force);

                }
            }
        } 
    }
}
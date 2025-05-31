
using GameEngine.ECS.Components;
using OpenTK.Mathematics;

namespace GameEngine.ECS.Systems
{
    public class PhysicsSystem : GameSystem
    {
        List<Entity> entities = new List<Entity>();
        Scene currentScene;


        public PhysicsSystem(Scene scene) : base(scene)
        {
            currentScene = scene;
        }


        public override void Update(float deltaTime)
        {
            entities = scene.GetEntitiesWith<RigidBody>();
            foreach (Entity entity in entities)
            {

                var entityMovement = entity.GetComponent<PlayerMovement>();
                RigidBody rb = entity.GetComponent<RigidBody>();
                if (rb != null & !rb.IsKinematic)
                {

                    Vector2 force = Vector2.Zero;
                    force.Y = -.003f;
                    rb.ApplyForce(force);

                }
            }
        } 
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.Egl;

namespace GameEngine.ECS
{
    public class Scene
    {
        private List<Entity> entities = new List<Entity>();

        public Entity CreateEntity(string name = "Entity")
        {
            Entity entity = new Entity(name);
            entities.Add(entity);
            return entity;

        }

        public Entity FindEntityByName(string name)
        {
            foreach (Entity entity in entities)
            { 
            if (entity.Name == name) return entity;
            }

            return null;
        }

        public void Destroy(Entity entity)
        {
            entity.Destroy();
            entities.Remove(entity);

        }

        public void Update(float deltaTime)
        {
            foreach(Entity entity in entities)
            {
                entity.Update(deltaTime);
            }
        }

        public void Render()
        {
            foreach (Entity entity in entities)
            {
                entity.Render();
            }
        }
    }
}

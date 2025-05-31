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
        public List<Entity> GetEntitiesWith<T>() where T : Component , new()
        {
            List<Entity> filteredEntities = new List<Entity>();
            foreach(Entity entity in entities)
            {
                if(entity.GetComponent<T>() != null) filteredEntities.Add(entity);
            }
            return filteredEntities;
        }

        public List<Entity> GetEntitiesWith<T1, T2, T3>() 
            where T1 : Component , new()
            where T2 : Component , new()
            where T3 : Component , new()
        {
            List<Entity> filterdEntities = new List<Entity>();

            foreach (Entity entity in entities)
            {
                var comp1 = entity.GetComponent<T1>();
                var comp2 = entity.GetComponent<T2>();
                var comp3 = entity.GetComponent<T3>();

                

                if (comp1 != null && comp2 != null && comp3 != null)
                {
                   
                    filterdEntities.Add(entity);
                }

            }
            return filterdEntities;
        }
        public List<Entity> GetEntitiesWith()
        {
            return this.entities;
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

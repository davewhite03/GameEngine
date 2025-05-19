using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ECS
{
    public class Entity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public List<Component> components = new List<Component>();

        public Entity(string name = "Entity")
        {
            Name = name;
        }

        public T AddComponent<T>() where T : Component, new()
        {

            T existingComponent = GetComponent<T>();

            if (existingComponent != null) {

                Console.WriteLine($"Entity '{Name}' already has a component of type {typeof(T).Name}");

                return existingComponent;
            }

            T component = new T();
            component.Entity = this;
            components.Add(component);
            component.Initialize();

            return component;

        }

        public T GetComponent<T>() where T : Component, new()
        {

            foreach (Component component in components)
            {
                if (component is T typedComponent)
                {
                    return typedComponent;
                }
            }
            return null;
        }


        public bool RemoveComponent<T>() where T : Component, new ()
        {
            for (int i = 0;i < components.Count; i++)
            {
                if (components[i] is T)
                {
                    components[i].Destroy();
                    components.RemoveAt(i);

                    return true;
                }
            }
            return false;
        }

        public void Update(float deltaTime)
        {
            if (!IsActive) return;

            foreach (Component component in components)
            {
                component.Update(deltaTime);
            }
        }

        public void Render()
        {
            if(!IsActive) return;

            foreach(Component component in components)
            {
                component.Render();
            }
        }

        public void Destroy()
        {
            if(!IsActive) return ;

            foreach (Component component in components)
            {
                component.Destroy();
            }
        }

    }
}

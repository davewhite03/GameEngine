using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ECS.Systems
{
    public class SystemManager
    {
        private List<GameSystem> systems = new List<GameSystem>();


        public void AddGameSystem(GameSystem system)
        {
            systems.Add(system);
            system.Initialize();
        }

        public void UpdateAll(float deltaTime)
        {
            foreach (GameSystem system in systems) {
            
            system.Update(deltaTime);
            }
        }


        public GameSystem GetSystem<T>() where T : GameSystem
        {
            foreach(GameSystem system in systems)
            {
                if (system.GetType() == typeof(T)) return system;
            }
            return null;
        }

        public void DestroyALl() 
        {

            foreach (GameSystem system in systems)
            { 
                system.Destroy();
            }
        
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Resources;

namespace GameEngine.ECS
{
    public abstract class GameSystem
    {
        protected Scene scene;

        public GameSystem(Scene scene)
        {
            this.scene = scene;
        }
        public abstract void Update(float deltaTime);
        

        public virtual void Initialize() { }

        public virtual void Destroy() { }

        
    }
}

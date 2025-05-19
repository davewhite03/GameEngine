using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ECS
{
    public abstract class Component
    {
        public Entity Entity { get; internal set; }

        public virtual void Initialize() { }

        public virtual void Update(float deltaTime) { }

        public virtual void Render() { }

        public virtual void Destroy() { }


    }
}

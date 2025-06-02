using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.ECS.Components
{
    public class PlayerInput : Component
    {
        public Vector2 MoveInput { get; set; } = Vector2.Zero;
        public bool JumpPressed { get; set; } = false;

        public bool JumpHeld { get; set; } = false;

        public List<string> Moves = new List<string>();   
        
    }
}

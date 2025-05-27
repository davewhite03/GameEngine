using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ECS.Components
{
    public class PlayerMovement : Component
    {
        public float MoveSpeed { get; set; } = 5.0f;
        public float JumpForce { get; set; } = 10.0f;
        public bool IsGrounded { get; set; } = false;
        public float GroundCheckDistance { get; set; } = 0.1f;
    }
}

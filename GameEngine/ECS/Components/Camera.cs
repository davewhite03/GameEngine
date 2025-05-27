using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace GameEngine.ECS.Components
{
    public class Camera : Component
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Zoom { get; set; } = 1.0f;

        public Entity Target { get; set; }

        public float FollowSpeed { get; set; } = 5.0f;

        public Vector2 Offset { get; set; } = Vector2.Zero;

        public bool SmoothFollow { get; set; } = true;

    }
}

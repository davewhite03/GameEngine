using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ECS.Components
{
    public class SpriteAnimation
    {
        public float FrameTime { get; set; } = 0.2f;
        public bool Loop { get; set; } = true;
        public int CurrentFrame { get; set; } = 0;   
        private float frameTimer = 0f;

        public void NextFrame()
        {
            CurrentFrame++;
            if (CurrentFrame >= 4)  // 4 frames in 2x2
            {
                CurrentFrame = Loop ? 0 : 3;
            }
        }
    }
}

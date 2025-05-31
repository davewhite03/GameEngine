using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace GameEngine.ECS.Components
{
    public class BoxCollider : Component
    {
        public Vector2 Size { get; set; } = new Vector2(1.0f, 1.0f);
        public Vector2 OffSet { get; set; } = Vector2.Zero;

        
        public enum CollisionType
        {
            Solid,
            Trigger,
            OneWay
        }
        public CollisionType Type { get; set; } = CollisionType.Solid;
        public bool IsTrigger => Type == CollisionType.Trigger;

        Transform transform;

        public override void Initialize() 
        {
            transform = Entity.GetComponent<Transform>();

            if (transform == null)
            {
            transform = Entity.AddComponent<Transform>();
            }
        }


        public RectangleF GetBounds()
        {
            Vector2 position = transform.Position + OffSet;
            Vector2 halfSize = Size * 0.5f * transform.Scale;
            
            return new RectangleF(
                position.X - halfSize.X,
                position.Y - halfSize.Y,
                Size.X * transform.Scale.X,
                Size.Y * transform.Scale.Y
                );
        }

        public bool Intersects( BoxCollider other )
        {
            RectangleF bounds = GetBounds();
            RectangleF otherBounds= other.GetBounds();

            return bounds.IntersectsWith( otherBounds );
        }
    }
}

using OpenTK.Mathematics;
using Vector2 = OpenTK.Mathematics.Vector2;

namespace GameEngine.ECS.Components
{
    public class Transform : Component
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0.0f;  
        public Vector2 Scale { get; set; } = new Vector2(1.0f,1.0f);

        public Matrix4 GetTransformationMatrix()
        {
            return Matrix4.CreateScale(Scale.X, Scale.Y, 1.0f) *
                   Matrix4.CreateRotationZ(Rotation) *
                   Matrix4.CreateTranslation(Position.X, Position.Y, 0.0f);
        }
    }
}

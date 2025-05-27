using System;
using System.Collections.Generic;
using System.Linq;

using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GameEngine.ECS.Components;
using OpenTK.Mathematics;

namespace GameEngine.ECS.Systems
{
    public class CameraSystem : GameSystem
    {
        public CameraSystem(Scene scene) : base(scene) {}

        
        public override void Update(float deltaTime)
        {
            var cameraEntities = scene.GetEntitiesWith<Camera>();

            foreach (var entity in cameraEntities)
            {

                var camera = entity.GetComponent<Camera>();

                if(camera.Target != null)
                {
                    UpdateCameraFollowing(camera, deltaTime);
                }
            }

        }

        private void UpdateCameraFollowing(Camera camera, float deltaTime)
        {
            var targetTransform = camera.Target.GetComponent<Transform>();

            if (targetTransform == null) return;

            Vector2 targetPosition = targetTransform.Position + camera.Offset;

            if (camera.SmoothFollow)
            {
                camera.Position = Vector2.Lerp(camera.Position, targetPosition, camera.FollowSpeed * deltaTime);
            }
            else
            {
                camera.Position = targetPosition;
            }
        }
    }
}

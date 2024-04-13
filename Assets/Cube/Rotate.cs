using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace WhyNot.Cube
{
    internal partial struct Rotate : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Rotation>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            var job = new RotateByAngle { DeltaTime = deltaTime };
            job.Schedule();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState _) { }

        private partial struct RotateByAngle : IJobEntity
        {
            [Required] public float DeltaTime { get; init; }

            private void Execute(ref LocalTransform transform, in Rotation rotation)
            {
                transform = transform.RotateY(math.radians(rotation.SpeedDegree) * DeltaTime);
            }
        }
    }
}

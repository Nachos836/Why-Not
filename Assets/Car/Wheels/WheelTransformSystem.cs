using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace WhyNot.Car.Wheels
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    internal partial struct WheelTransformSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new UpdateTransformJob();
            job.Schedule();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        private partial struct UpdateTransformJob : IJobEntity
        {
            [BurstCompile]
            private static void Execute(in WheelOrigin origin, in WheelContact contact, ref LocalTransform transform)
            {
                transform.Position = origin.Value.pos
                                   - math.rotate(origin.Value.rot, math.up()) * contact.Distance;
                transform.Rotation = origin.Value.rot;
            }
        }
    }
}

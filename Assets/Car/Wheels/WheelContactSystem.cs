using Unity.Burst;
using Unity.Entities;

namespace WhyNot.Car.Wheels
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(VehicleToWheelSystem))]
    public partial struct WheelContactSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
}

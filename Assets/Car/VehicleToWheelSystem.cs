using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace WhyNot.Car
{
    using Wheels;

    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct VehicleToWheelSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.Enabled = false;
            state.RequireForUpdate<Vehicle>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (vehicle, transform) in SystemAPI.Query<RefRO<Vehicle>, RefRO<LocalTransform>>())
            {
                foreach (var wheel in vehicle.ValueRO.Wheels)
                {
                    var origin = SystemAPI.GetComponent<WheelOrigin>(wheel);
                    var wheelTransform = math.mul
                    (
                        new RigidTransform(transform.ValueRO.Rotation, transform.ValueRO.Position),
                        origin.Value
                    );
                    var wheelInput = new WheelInput
                    {
                        Up = math.rotate(wheelTransform.rot, math.up()),
                        WorldTransform = wheelTransform
                    };

                    SystemAPI.SetComponent(wheel, wheelInput);
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState _) { }
    }
}

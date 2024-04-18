using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace WhyNot.Car.Wheels
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct WheelToVehicleSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Vehicle>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (origin, parent, entity) in SystemAPI.Query<RefRO<WheelOrigin>, RefRO<Parent>>().WithEntityAccess())
            {
                var vehicle = state.EntityManager.GetComponentData<LocalTransform>(parent.ValueRO.Value);

                var wheelTransform = math.mul
                (
                    new RigidTransform(vehicle.Rotation, vehicle.Position),
                    origin.ValueRO.Value
                );

                state.EntityManager.SetComponentData(entity, new WheelInput
                {
                    Up = math.rotate(wheelTransform.rot, math.up()),
                    WorldTransform = wheelTransform
                });
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
}

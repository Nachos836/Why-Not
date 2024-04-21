using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace WhyNot.Car
{
    using Input;

    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    internal partial struct CarDebugDragging : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAny<InputBuffer>();

            state.RequireForUpdate(state.GetEntityQuery(builder));
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            foreach (var buffer in SystemAPI.Query<DynamicBuffer<InputBuffer>>())
            {
                foreach (var candidate in buffer)
                {
                    if (physicsWorld.CastRay(candidate.Raycast, out var hit))
                    {
                        Debug.Log($"{hit.Position}");
                    }
                }

                buffer.Clear();
            }
        }

        public void OnDestroy(ref SystemState state) { }
    }
}

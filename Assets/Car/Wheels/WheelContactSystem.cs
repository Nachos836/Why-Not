using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace WhyNot.Car.Wheels
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(VehicleToWheelSystem))]
    public partial struct WheelContactSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            var job = new ContactJob { PhysicsWorld = physicsWorld.PhysicsWorld };
            job.Schedule();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        private partial struct ContactJob : IJobEntity
        {
            [Required] [field: ReadOnly] public PhysicsWorld PhysicsWorld { get; init; }

            [BurstCompile]
            private void Execute(in Wheel wheel, in WheelInput input, ref WheelContact contact)
            {
                unsafe
                {
                    var colliderCastInput = new ColliderCastInput
                    {
                        Collider = (Collider*) wheel.Collider.GetUnsafePtr(),
                        Orientation = input.WorldTransform.rot,
                        Start = input.WorldTransform.pos,
                        End = input.WorldTransform.pos - input.Up * wheel.SuspensionLength,
                        QueryColliderScale = 0
                    };

                    if (PhysicsWorld.CastCollider(colliderCastInput, out var hit) is not true)
                    {
                        contact.IsInContact = false;
                        contact.Distance = wheel.SuspensionLength;

                        return;
                    }

                    contact.IsInContact = true;
                    contact.Point = hit.Position;
                    contact.Normal = hit.SurfaceNormal;
                    contact.Distance = hit.Fraction * wheel.SuspensionLength;
                }
            }
        }
    }
}

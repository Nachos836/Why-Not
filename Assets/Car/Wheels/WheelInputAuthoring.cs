using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WhyNot.Car.Wheels
{
    [BurstCompile]
    internal struct WheelInput : IComponentData
    {
        [Required] public float3 Up;
        [Required] public RigidTransform WorldTransform;
    }

    internal sealed class WheelInputAuthoring : MonoBehaviour
    {
        internal sealed class WheelInputBaker : Baker<WheelInputAuthoring>
        {
            public override void Bake(WheelInputAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new WheelInput
                {
                    Up = default,
                    WorldTransform = default
                });
            }
        }
    }
}

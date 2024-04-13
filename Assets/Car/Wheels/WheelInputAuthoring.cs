using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WhyNot.Car.Wheels
{
    [BurstCompile]
    public struct WheelInput : IComponentData
    {
        [Required] public float3 Up { get; init; }
        [Required] public RigidTransform WorldTransform { get; init; }
    }

    public class WheelInputAuthoring : MonoBehaviour
    {
        public class WheelInputBaker : Baker<WheelInputAuthoring>
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

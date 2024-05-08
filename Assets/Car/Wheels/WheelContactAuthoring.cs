using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WhyNot.Car.Wheels
{
    [BurstCompile]
    internal struct WheelContact : IComponentData
    {
        [Required] public bool IsInContact;
        [Required] public float3 Point;
        [Required] public float3 Normal;
        [Required] public float Distance;
    }

    internal sealed class WheelContactAuthoring : MonoBehaviour
    {
        internal sealed class WheelContactBaker : Baker<WheelContactAuthoring>
        {
            public override void Bake(WheelContactAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new WheelContact
                {
                    IsInContact = false,
                    Point = default,
                    Normal = default,
                    Distance = default
                });
            }
        }
    }
}

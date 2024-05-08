using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WhyNot.Car.Wheels
{
    [BurstCompile]
    internal struct WheelOrigin : IComponentData
    {
        [Required] public RigidTransform Value;
    }

    internal sealed class WheelOriginAuthoring : MonoBehaviour
    {
        internal sealed class WheelOriginBaker : Baker<WheelOriginAuthoring>
        {
            public override void Bake(WheelOriginAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var transform = authoring.transform;

                AddComponent(entity, new WheelOrigin
                {
                    Value = new RigidTransform
                    {
                        pos = transform.localPosition,
                        rot = transform.localRotation
                    }
                });
            }
        }
    }
}

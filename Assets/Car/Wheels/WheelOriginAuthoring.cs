using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WhyNot.Car.Wheels
{
    [BurstCompile]
    public struct WheelOrigin : IComponentData
    {
        [Required] public RigidTransform Value;
    }

    public class WheelOriginAuthoring : MonoBehaviour
    {
        public class WheelOriginBaker : Baker<WheelOriginAuthoring>
        {
            public override void Bake(WheelOriginAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new WheelOrigin
                {
                    Value = new RigidTransform
                    {
                        pos = authoring.transform.localPosition,
                        rot = authoring.transform.localRotation
                    }
                });
            }
        }
    }
}

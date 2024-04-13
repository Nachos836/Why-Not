using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics;

using Collider = Unity.Physics.Collider;

namespace WhyNot.Car.Wheels
{
    [BurstCompile]
    public struct Wheel : IComponentData
    {
        [Required] public float Radius { get; init; }
        [Required] public float Width { get; init; }
        [Required] public float SuspensionLength { get; init; }
        [Required] public BlobAssetReference<Collider> Collider { get; init; }
    }

    [RequireComponent(typeof(WheelOriginAuthoring), typeof(WheelInputAuthoring))]
    public class WheelAuthoring : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _width;
        [SerializeField] private float _suspensionLength;

        public class WheelBaker : Baker<WheelAuthoring>
        {
            public override void Bake(WheelAuthoring authoring)
            {
                var collider = CylinderCollider.Create(new CylinderGeometry
                {
                    Center = float3.zero,
                    Orientation = quaternion.AxisAngle(math.up(), math.PI * 0.5f),
                    Height = authoring._width,
                    Radius = authoring._radius,
                    BevelRadius = 0.01f,
                    SideCount = 12
                });

                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Wheel
                {
                    Radius = authoring._radius,
                    Width = authoring._width,
                    SuspensionLength = authoring._suspensionLength,
                    Collider = collider
                });
            }
        }
    }
}

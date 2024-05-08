using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

using Collider = Unity.Physics.Collider;

namespace WhyNot.Car.Wheels
{
    [BurstCompile]
    internal struct Wheel : IComponentData
    {
        [Required] public float Radius;
        [Required] public float Width;
        [Required] public float SuspensionLength;
        [Required] public BlobAssetReference<Collider> Collider;
    }

    [RequireComponent(typeof(WheelOriginAuthoring), typeof(WheelInputAuthoring), typeof(WheelContactAuthoring))]
    internal sealed class WheelAuthoring : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _width;
        [Header("Collider")]
        [SerializeField] private float _suspensionLength;
        [Header("Suspension")]
        [SerializeField] private LayerMask _belongsTo;
        [SerializeField] private LayerMask _collidesWith;

        internal sealed class WheelBaker : Baker<WheelAuthoring>
        {
            public override void Bake(WheelAuthoring authoring)
            {
                var collider = CylinderCollider.Create
                (
                    new CylinderGeometry
                    {
                        Center = float3.zero,
                        Orientation = quaternion.AxisAngle(math.up(), math.PI * 0.5f),
                        Height = authoring._width,
                        Radius = authoring._radius,
                        BevelRadius = 0.01f,
                        SideCount = 12
                    },
                    new CollisionFilter
                    {
                        BelongsTo = (uint) authoring._belongsTo.value,
                        CollidesWith = ~0u & ~ (uint) authoring._collidesWith.value,
                        GroupIndex = 0
                    }
                );

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

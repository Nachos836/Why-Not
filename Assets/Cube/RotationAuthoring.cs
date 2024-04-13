using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace WhyNot.Cube
{
    [BurstCompile]
    internal struct Rotation : IComponentData
    {
        [Required] public float SpeedDegree { get; init; }
    }

    internal sealed class RotationAuthoring : MonoBehaviour
    {
        [SerializeField] private float _speedDegree = 360;

        public class RotationBaker : Baker<RotationAuthoring>
        {
            public override void Bake(RotationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Rotation
                {
                    SpeedDegree = authoring._speedDegree
                });
            }
        }
    }
}

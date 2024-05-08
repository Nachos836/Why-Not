using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace WhyNot.Car
{
    [BurstCompile]
    internal struct Vehicle : IComponentData
    {
    }

    internal sealed class VehicleAuthoring : MonoBehaviour
    {
        internal sealed class VehicleBaker : Baker<VehicleAuthoring>
        {
            public override void Bake(VehicleAuthoring authoring)
            {
                var vehicle = new Vehicle { };

                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, vehicle);
            }
        }
    }
}

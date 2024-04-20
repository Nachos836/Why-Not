using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace WhyNot.Car
{
    [BurstCompile]
    public struct Vehicle : IComponentData
    {
    }

    public class VehicleAuthoring : MonoBehaviour
    {
        public class VehicleBaker : Baker<VehicleAuthoring>
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

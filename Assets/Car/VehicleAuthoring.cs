using System;
using System.ComponentModel.DataAnnotations;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using WhyNot.Car.Wheels;

namespace WhyNot.Car
{
    [BurstCompile]
    public struct Vehicle : IComponentData
    {
        [Required] public FixedList64Bytes<Entity> Wheels;
    }

    public class VehicleAuthoring : MonoBehaviour
    {
        [SerializeField] private WheelAuthoring[] _wheels = Array.Empty<WheelAuthoring>();

        private void Reset()
        {
            _wheels = GetComponentsInChildren<WheelAuthoring>();
        }

        public class VehicleBaker : Baker<VehicleAuthoring>
        {
            public override void Bake(VehicleAuthoring authoring)
            {
                var vehicle = new Vehicle { Wheels = new FixedList64Bytes<Entity>() };

                foreach (var wheelAuthoring in authoring._wheels)
                {
                    vehicle.Wheels.Add(GetEntity(wheelAuthoring, TransformUsageFlags.Dynamic));
                }

                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, vehicle);
            }
        }
    }
}

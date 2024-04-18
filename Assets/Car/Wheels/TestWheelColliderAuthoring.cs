using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace WhyNot.Car.Wheels
{
    public struct TestWheelCollider : IComponentData
    {

    }

    internal sealed class TestWheelColliderAuthoring : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.32f;
        [SerializeField] private float _width = 0.25f;

        internal sealed class TestWheelColliderBaker : Baker<TestWheelColliderAuthoring>
        {
            public override void Bake(TestWheelColliderAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<TestWheelCollider>(entity);

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
                    }
                );

                AddComponent(entity, new PhysicsCollider
                {
                    Value = collider
                });
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WhyNot.Input
{
    internal sealed class InputProvider : MonoBehaviour
    {
        private InputActions _inputActions = default;
        private InputActions.DebugActions _debugInputs = default;
        private Entity _entity = default;
        private World _world;
        private EntityManager _entityManager;

        private void Awake()
        {
            _inputActions = new InputActions();
            _debugInputs = _inputActions.Debug;
            _world = World.DefaultGameObjectInjectionWorld;
            _entityManager = _world.EntityManager;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _debugInputs.Enable();
            _debugInputs.MousePick.performed += HandleMousePick;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
            _debugInputs.MousePick.performed -= HandleMousePick;
            _debugInputs.Disable();

            if (_world.IsCreated && !_entityManager.Exists(_entity))
            {
                _entityManager.DestroyEntity(_entity);
            }
        }

        private void HandleMousePick(InputAction.CallbackContext context)
        {
            var mousePosition = context.ReadValue<Vector2>();
            Debug.Log($"Mouse position: {mousePosition}");
            RaycastInput raycastInput = new()
            {
                Start = Camera.main.ScreenPointToRay(mousePosition).origin,
                End = Camera.main.ScreenPointToRay(mousePosition).direction * 1000f,
                Filter = CollisionFilter.Default
            };

            if (_world.IsCreated && !_entityManager.Exists(_entity))
            {
                _entity = _entityManager.CreateEntity();
                _entityManager.AddBuffer<InputBuffer>(_entity);
            }
            _entityManager.GetBuffer<InputBuffer>(_entity).Add(new InputBuffer { Raycast = raycastInput });
        }
    }

    public struct InputBuffer : IBufferElementData
    {
        [Required] public RaycastInput Raycast;
    }
}

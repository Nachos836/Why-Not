using System.ComponentModel.DataAnnotations;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WhyNot.Input
{
    internal sealed class InputProvider : MonoBehaviour
    {
        private InputActions _inputActions = default!;
        private InputActions.DebugActions _debugInputs = default!;
        private Entity _entity = Entity.Null;
        private World _world = default!;
        private EntityManager _entityManager = default!;
        private InputAction _mousePick = default!;
        private InputAction _mousePosition = default!;

        private void Awake()
        {
            _inputActions = new InputActions();
            _debugInputs = _inputActions.Debug;
            _mousePick = _debugInputs.MousePick;
            _mousePosition = _debugInputs.MousePosition;
            _world = World.DefaultGameObjectInjectionWorld;
            _entityManager = _world.EntityManager;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _debugInputs.Enable();
            _mousePick.Enable();
            _mousePosition.Enable();

            _mousePosition.performed += HandleMousePick;
        }

        private void OnDisable()
        {
            _mousePosition.performed -= HandleMousePick;

            _mousePosition.Disable();
            _mousePick.Disable();
            _inputActions.Disable();
            _debugInputs.Disable();

            if (_world.IsCreated && !_entityManager.Exists(_entity))
            {
                _entityManager.DestroyEntity(_entity);
            }
        }

        private void HandleMousePick(InputAction.CallbackContext context)
        {
            var mousePosition = context.ReadValue<Vector2>();
            var currentCamera = Camera.main!;
            var ray = currentCamera.ScreenPointToRay(mousePosition);

            Debug.Log($"Mouse position: {mousePosition}");
            RaycastInput raycastInput = new ()
            {
                Start = ray.origin,
                End = ray.GetPoint(currentCamera.farClipPlane),
                Filter = CollisionFilter.Default
            };

            if (_world.IsCreated && !_entityManager.Exists(_entity))
            {
                _entity = _entityManager.CreateEntity();
                _entityManager.AddBuffer<InputBuffer>(_entity);
            }

            _entityManager.GetBuffer<InputBuffer>(_entity)
                .Add(new InputBuffer { Raycast = raycastInput });
        }
    }

    public struct InputBuffer : IBufferElementData
    {
        [Required] public RaycastInput Raycast;
    }
}

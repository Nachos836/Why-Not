using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WhyNot.Input
{
    internal sealed class InputProvider : MonoBehaviour
    {
        private InputActions _inputActions = default!;
        private InputActions.DebugActions _debugInputs;
        private Entity _entity = Entity.Null;
        private World _world = default!;
        private EntityManager _entityManager;
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

            _mousePick.started += HandleStartMousePick;
            _mousePick.canceled += HandleEndMousePick;
        }

        private void OnDisable()
        {
            _mousePick.canceled -= HandleEndMousePick;
            _mousePick.started -= HandleStartMousePick;

            _mousePosition.Disable();
            _mousePick.Disable();
            _inputActions.Disable();
            _debugInputs.Disable();

            if (_world.IsCreated && !_entityManager.Exists(_entity))
            {
                _entityManager.DestroyEntity(_entity);
            }
        }

        private void HandleEndMousePick(InputAction.CallbackContext obj)
        {
            if (_entity != Entity.Null)
            {
                _entityManager.SetComponentEnabled<InputGathered>(_entity, true);
            }

            _mousePosition.performed -= HandleMousePosition;
        }

        private void HandleStartMousePick(InputAction.CallbackContext obj)
        {
            if (_entity != Entity.Null)
            {
                _entityManager.SetComponentEnabled<InputGathered>(_entity, false);
            }

            _mousePosition.performed += HandleMousePosition;
        }

        private void HandleMousePosition(InputAction.CallbackContext context)
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

                using var commands = new EntityCommandBuffer(Allocator.Temp);

                commands.AddBuffer<InputBuffer>(_entity);
                commands.AddComponent<InputGathered>(_entity);
                commands.SetComponentEnabled<InputGathered>(_entity, false);

                commands.Playback(_entityManager);
            }

            _entityManager.GetBuffer<InputBuffer>(_entity)
                .Add(new InputBuffer { Raycast = raycastInput });
        }
    }
}

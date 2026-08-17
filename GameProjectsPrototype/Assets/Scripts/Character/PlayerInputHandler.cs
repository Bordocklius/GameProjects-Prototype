using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Character
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;

        public event EventHandler<Vector2> Move;
        public event EventHandler<Vector2> Look;
        public event EventHandler Jump;
        public event EventHandler Attack;
        public event EventHandler Interact;

        public event EventHandler SubmitCommand;
        public event EventHandler CancelCommand;

        public bool InCommandMode {  get; private set; }

        private void Awake()
        {
            if(_playerInput == null)
                _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.actions["Move"].performed += PlayerInput_Move;
            _playerInput.actions["Move"].canceled += PlayerInput_Move;

            _playerInput.actions["Jump"].performed += PlayerInput_Jump;

            _playerInput.actions["Look"].performed += PlayerInput_Look;
            _playerInput.actions["Look"].canceled += PlayerInput_Look;

            _playerInput.actions["Attack"].performed += PlayerInput_Attack;

            _playerInput.actions["Interact"].performed += PlayerInput_Interact;

            _playerInput.actions["Submit"].performed += PlayerInput_Submit;
            _playerInput.actions["Cancel"].performed += PlayerInput_Cancel;
        }

        private void OnDisable()
        {
            _playerInput.actions["Move"].performed -= PlayerInput_Move;
            _playerInput.actions["Move"].canceled -= PlayerInput_Move;

            _playerInput.actions["Jump"].performed -= PlayerInput_Jump;

            _playerInput.actions["Look"].performed -= PlayerInput_Look;
            _playerInput.actions["Look"].canceled -= PlayerInput_Look;

            _playerInput.actions["Attack"].performed -= PlayerInput_Attack;

            _playerInput.actions["Interact"].performed -= PlayerInput_Interact;

            _playerInput.actions["Submit"].performed -= PlayerInput_Submit;
            _playerInput.actions["Cancel"].performed -= PlayerInput_Cancel;
        }  
        
        public void SwitchToPlayMode()
        {
            InCommandMode = false;
            _playerInput.SwitchCurrentActionMap("Player");
            Debug.Log($"Current action map: {_playerInput.currentActionMap.name}");
        }

        public void SwitchToCommandMode()
        {
            InCommandMode = true;
            _playerInput.SwitchCurrentActionMap("Command");
            Debug.Log($"Current action map: {_playerInput.currentActionMap.name}");
        }
        
        private void PlayerInput_Move(InputAction.CallbackContext ctx)
        {
            Move?.Invoke(this, ctx.ReadValue<Vector2>());
        }

        private void PlayerInput_Jump(InputAction.CallbackContext ctx)
        {
            Jump?.Invoke(this, EventArgs.Empty);
        }

        private void PlayerInput_Look(InputAction.CallbackContext ctx)
        {
            Look?.Invoke(this, ctx.ReadValue<Vector2>());
        }
        private void PlayerInput_Attack(InputAction.CallbackContext ctx)
        {
            Attack?.Invoke(this, EventArgs.Empty);
        }

        private void PlayerInput_Submit(InputAction.CallbackContext ctx)
        {
            SubmitCommand?.Invoke(this, EventArgs.Empty);
        }

        private void PlayerInput_Cancel(InputAction.CallbackContext ctx)
        {
            CancelCommand?.Invoke(this, EventArgs.Empty);
        }

        private void PlayerInput_Interact(InputAction.CallbackContext ctx)
        {
            Interact?.Invoke(this, EventArgs.Empty);
        }
    }
}

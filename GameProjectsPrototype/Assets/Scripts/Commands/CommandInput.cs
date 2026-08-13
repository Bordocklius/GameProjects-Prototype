using Assets.Scripts.Character;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Commands
{
    public class CommandInput : MonoBehaviour
    {
        public static CommandInput Instance { get; private set; }

        [SerializeField] private TMP_InputField _inputfield;
        [SerializeField] private PlayerInputHandler _playerInputHandler;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Optional: keep this across scene loads
            // DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            //_playerInputHandler.SubmitCommand += PlayerInput_SubmitCommand;
            _playerInputHandler.CancelCommand += PlayerInput_CancelCommand;
        }

        private void OnDisable()
        {
            //_playerInputHandler.SubmitCommand -= PlayerInput_SubmitCommand;
            _playerInputHandler.CancelCommand -= PlayerInput_CancelCommand;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void Activate()
        {
            _playerInputHandler.SwitchToCommandMode();
            _inputfield.gameObject.SetActive(true);

            _inputfield.text = "";
            _inputfield.Select();
            _inputfield.ActivateInputField();
            _inputfield.caretPosition = _inputfield.text.Length;
        }

        public void Deactivate()
        {
            _inputfield.DeactivateInputField();
            EventSystem.current.SetSelectedGameObject(null);
            _playerInputHandler.SwitchToPlayMode();

            _inputfield.text = "";
            _inputfield.gameObject.SetActive(false);
        }

        public void PlayerInput_SubmitCommand()
        {
            string input = _inputfield.text;

            CommandSystem.Instance.ExecuteCommand(input);

            Deactivate();            
        }

        public void PlayerInput_SubmitCommand(object sender, EventArgs e)
        {
            PlayerInput_SubmitCommand();
        }

        public void PlayerInput_CancelCommand(object sender, EventArgs e)
        {
            Deactivate();
        }
    }
}

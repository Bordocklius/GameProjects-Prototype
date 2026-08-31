using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class CommandSystem : MonoBehaviour
    {
        public static CommandSystem Instance { get; private set; }

        [SerializeField] private CommandBase[] _commands;

        private GameObject _target;

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

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        public GameObject GetTarget() { return _target; }

        public void ExecuteCommand(string input)
        {
            // Check if there is a target and if the input is valid text
            if(_target == null)
            {
                Debug.Log("No target");
                return;
            }
            if(string.IsNullOrEmpty(input))
            {
                Debug.Log("No command inputted");
                return;
            }

            // Find command in command list
            CommandBase command = FindComand(input);
            if(command == null)
            {
                Debug.Log("No command found");
                return;
            }

            // Check if target can execute command
            if (!command.CanExecuteCommand(_target))
            {
                Debug.Log($"Target cannot execute {command.CommandWord}");
                return;
            }

            // Execute and reset target
            command.ExecuteCommand(_target);
            Debug.Log($"{input} command executed");
            _target = null;
        }

        private CommandBase FindComand(string input)
        {
            input = input.Trim().ToLower();

            return _commands.FirstOrDefault(x => x.CommandWord.Trim().ToLower() == input);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class CommandSystem : MonoBehaviour
    {
        [SerializeField] private CommandBase[] _commands;

        private GameObject _target;

        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        public void ExecuteCommand(string input)
        {
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

            CommandBase command;
        }

        private CommandBase FindComand(string input)
        {
            input = input.Trim().ToLower();

            return _commands.FirstOrDefault(x => x.CommandWord.Trim().ToLower() == input);
        }
    }
}

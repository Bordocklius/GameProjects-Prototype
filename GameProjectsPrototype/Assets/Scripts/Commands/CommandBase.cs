using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public abstract class CommandBase : ScriptableObject
    {
        [Header("Command Word")]
        public string CommandWord;

        public abstract bool CanExecuteCommand(GameObject target);

        public abstract void ExecuteCommand(GameObject target);
    }
}

using Assets.Scripts.Commandables;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    [CreateAssetMenu(fileName = "LightenCommand", menuName = "Commands/Lighten Command")]
    public class LightenCommand : CommandBase
    {
        public override bool CanExecuteCommand(GameObject target)
        {
            return target.TryGetComponent<Massable>(out _);
        }

        public override void ExecuteCommand(GameObject target)
        {
            if (target.TryGetComponent<Massable>(out var massable))
            {
                massable.DecreaseMass(); ;
            }
        }
    }
}

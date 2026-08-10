using Assets.Scripts.Commandables;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    [CreateAssetMenu(fileName = "ShrinkCommand", menuName = "Commands/Shrink Command")]
    public class ShrinkCommand : CommandBase
    {
        public override bool CanExecuteCommand(GameObject target)
        {
            return target.TryGetComponent<Scalable>(out _);
        }

        public override void ExecuteCommand(GameObject target)
        {
            if (!target.TryGetComponent<Scalable>(out var growable))
                return;

            growable.Shrink();
        }
    }
}

using Assets.Scripts.Commandables;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    [CreateAssetMenu(fileName = "GrowCommand", menuName = "Commands/Grow Command")]
    public class GrowthCommand : CommandBase
    {
        public override bool CanExecuteCommand(GameObject target)
        {
            return target.TryGetComponent<ICommandTarget>(out _);
        }

        public override void ExecuteCommand(GameObject target)
        {
            if(!target.TryGetComponent<Scalable>(out var scalable))
            {
                scalable.Grow();
            }
        }
    }
}

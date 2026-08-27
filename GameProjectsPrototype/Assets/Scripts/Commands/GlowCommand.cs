using Assets.Scripts.Commandables;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    [CreateAssetMenu(fileName = "GlowCommand", menuName = "Commands/Glow Command")]
    public class GlowCommand : CommandBase
    {
        public override bool CanExecuteCommand(GameObject target)
        {
            return target.TryGetComponent<Glowable>(out _);
        }

        public override void ExecuteCommand(GameObject target)
        {
            if (target.TryGetComponent<Glowable>(out Glowable glowable))
            {
                glowable.EnableGlow();
            }
        }
    }
}

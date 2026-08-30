using Assets.Scripts.Commandables;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    [CreateAssetMenu(fileName = "ThrowCommand", menuName = "Commands/Throw command")]
    public class ThrowCommand : CommandBase
    {
        public override bool CanExecuteCommand(GameObject target)
        {
            return target.TryGetComponent<Throwable>(out _);
        }

        public override void ExecuteCommand(GameObject target)
        {
            if (!target.TryGetComponent<Throwable>(out var stickable))
                return;

            stickable.Throw();
        }
    }
}

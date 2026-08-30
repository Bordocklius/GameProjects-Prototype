using Assets.Scripts.Commandables;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    [CreateAssetMenu(fileName = "NotStickyCommand", menuName = "Commands/Not sticky command")]
    public class NotStickyCommand : CommandBase
    {
        public override bool CanExecuteCommand(GameObject target)
        {
            return target.TryGetComponent<Stickable>(out _);
        }

        public override void ExecuteCommand(GameObject target)
        {
            if (!target.TryGetComponent<Stickable>(out var stickable))
                return;

            stickable.MakeNotSticky();
        }
    }
}

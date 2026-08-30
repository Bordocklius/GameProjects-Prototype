using Assets.Scripts.Commandables;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    [CreateAssetMenu(fileName = "StickyCommand", menuName = "Commands/Sticky command")]
    public class StickyCommand : CommandBase
    {
        public override bool CanExecuteCommand(GameObject target)
        {
            return target.TryGetComponent<Stickable>(out _);
        }

        public override void ExecuteCommand(GameObject target)
        {
            if (!target.TryGetComponent<Stickable>(out var stickable))
                return;

            stickable.MakeSticky();
        }
    }
}

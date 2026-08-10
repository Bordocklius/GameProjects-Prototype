
namespace Assets.Scripts.Interfaces
{
    public enum CodeWord
    {
        Destroy,
        Mass,
        Lighten,
        Glow,
        Stick,
        Grow,
        Shrink,
        Throw,
        Cancel,
        Rotate
    }

    public interface IInteractable
    {
        public void Interact(CodeWord codeWord);
    }
}

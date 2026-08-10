
using Assets.Scripts.Commands;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ICommandTarget
    {
        public bool TryGetCapability<T>(out T capability) where T : Component;
    }
}

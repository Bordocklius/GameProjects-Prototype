using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commandables
{
    public class CommandTarget : MonoBehaviour, ICommandTarget
    {
        public bool TryGetCapability<T>(out T capability) where T : Component
        {
            return TryGetComponent<T>(out capability);
        }
    }
}

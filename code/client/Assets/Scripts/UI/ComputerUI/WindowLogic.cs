using System;
using UnityEngine;

namespace UI
{
    public abstract class WindowLogic : MonoBehaviour
    {
        public ComputerWindow ComputerWindow;

        protected virtual void Awake()
        {
            ComputerWindow = GetComponentInChildren<ComputerWindow>();
        }

       
    }
}
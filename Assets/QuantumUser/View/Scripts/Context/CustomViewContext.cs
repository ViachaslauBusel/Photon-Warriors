using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;

namespace Quantum.Context
{
    public class CustomViewContext : MonoBehaviour, IQuantumViewContext
    {
        [SerializeField]
        private Camera _camera;

        public Camera Camera => _camera;
    }
}
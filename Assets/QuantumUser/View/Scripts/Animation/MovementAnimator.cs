using Quantum.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Quantum.Animation
{
    public class MovementAnimator : QuantumEntityViewComponent<CustomViewContext>
    {
        private Animator _animator;

        public override void OnInitialize()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public override void OnUpdateView()
        {
            var characterController = PredictedFrame.Get<CharacterController3D>(EntityRef);
        }
    }
}

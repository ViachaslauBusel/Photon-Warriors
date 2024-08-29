using Quantum.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quantum.Character
{
    public class CharacterCameraFollow : QuantumEntityViewComponent<CustomViewContext>
    {
        [SerializeField]
        private Vector3 _offset;
        [SerializeField]
        private float _smoothTime;
        private bool _localPlayer;
        private Vector3 _velocity;

        public override void OnActivate(Frame frame)
        {
            var link = frame.Get<PlayerLink>(EntityRef);
            _localPlayer = Game.PlayerIsLocal(link.Player);

            if (_localPlayer)
            {
                ViewContext.Camera.transform.position = transform.position + _offset;
                ViewContext.Camera.transform.LookAt(transform.position);
            }
        }

        public override void OnUpdateView()
        {
            if (_localPlayer == false) return;

            Vector3 targetPosition = transform.position + _offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(ViewContext.Camera.transform.position, targetPosition, ref _velocity, _smoothTime);
            ViewContext.Camera.transform.position = smoothedPosition;
            ViewContext.Camera.transform.LookAt(transform.position);
        }
    }
}

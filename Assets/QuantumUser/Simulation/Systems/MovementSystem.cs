namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.playerLink->Player);
            var direction = input->Direction;

            FP magnitudeOfDirection = direction.Magnitude;

            if (magnitudeOfDirection < FP._0_01) return;

            if (magnitudeOfDirection > 1)
            {
                direction = direction.Normalized;
            }

            filter.controller->MaxSpeed = filter.stats->MovementSpeed;
            filter.controller->Move(f, filter.Entity, direction.XOY);
            filter.transform->Rotation = FPQuaternion.LookRotation(direction.XOY);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* transform;
            public CharacterController3D* controller;
            public PlayerLink* playerLink;
            public PlayerStats* stats;
        }
    }
}

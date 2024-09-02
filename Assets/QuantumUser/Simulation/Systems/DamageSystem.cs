namespace Quantum
{
    using Photon.Deterministic;
    using System.Collections.Generic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class DamageSystem : SystemMainThreadFilter<DamageSystem.Filter>
    {
        private List<KeyValuePair<FP, EntityRef>> _enemiesInRange = new List<KeyValuePair<FP, EntityRef>>();

        public override void Update(Frame f, ref Filter filter)
        {
            var enimeies = f.Filter<Transform3D, EntityHealth>();
            _enemiesInRange.Clear();

            // Find all enemies in range
            while (enimeies.Next(out EntityRef enemy, out Transform3D enemyTransform, out EntityHealth enemyHealth))
            {
                if (enemyHealth.Health <= 0) continue;

                var distance = (filter.transform->Position - enemyTransform.Position).Magnitude;

                if (distance < filter.stats->AttackRange)
                {
                    _enemiesInRange.Add(new KeyValuePair<FP, EntityRef>(distance, enemy));
                }
            }

            // Sort by distance
            _enemiesInRange.Sort((x, y) => x.Key.CompareTo(y.Key));

            // Damage first 3 enemies
            for (var i = 0; i < 3 && i < _enemiesInRange.Count; i++)
            {
                var enemy = _enemiesInRange[i].Value;
                if (f.Unsafe.TryGetPointer(enemy, out EntityHealth* health))
                {
                    health->Health -= filter.stats->Damage * f.DeltaTime;

                    // Check if enemy is dead
                    if (health->Health <= 0)
                    {
                        f.Destroy(enemy);

                        // Increase player kills
                        if (f.Unsafe.TryGetPointer(filter.Entity, out PlayerKillsHolder* playerKills))
                        {
                            playerKills->KillsCouner++;
                            f.Events.PlayerKillsUpdated(filter.playerLink->Player, playerKills->KillsCouner);
                        }
                    }
                    else f.Events.HealthUpdated(enemy, health->Health, health->MaxHealth);
                }
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* playerLink;
            public Transform3D* transform;
            public DamageDealer* damageDealer;
            public PlayerStats* stats;
        }
    }
}

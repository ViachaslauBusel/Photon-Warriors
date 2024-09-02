namespace Quantum
{
    using Quantum.Commands;
    using Quantum.Data;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerStatsUpdateSystem : SystemMainThreadFilter<PlayerStatsUpdateSystem.Filter>, ISignalOnComponentAdded<PlayerLink>
    {
        public void OnAdded(Frame f, EntityRef entity, PlayerLink* component)
        {
            var player = component->Player;
            if (f.Unsafe.TryGetPointer(entity, out PlayerStats* stats))
            {
                var upgraders = f.FindAsset(stats->Upgraders);

                stats->MovementSpeed = upgraders.GetBaseValue(StatType.MovementSpeed);
                stats->Damage = upgraders.GetBaseValue(StatType.Damage);
                stats->AttackRange = upgraders.GetBaseValue(StatType.AttackRange);

                f.Events.MainPlayerStatsUpdated(player, stats->MovementSpeed, stats->Damage, stats->AttackRange);
            }
        }

        public override void Update(Frame f, ref Filter filter)
        {
           var command = f.GetPlayerCommand(filter.playerLink->Player) as UpgradePlayerStatsCommand;

            if (command != null)
            {
                var upgraders = f.FindAsset(filter.playerStats->Upgraders);
                var upgrade = upgraders.GetRandomStatUpgrader(f);

                switch (upgrade.StatType)
                {
                    case StatType.MovementSpeed:
                        filter.playerStats->MovementSpeed += upgrade.UpgradeValue;
                        break;
                    case StatType.Damage:
                        filter.playerStats->Damage += upgrade.UpgradeValue;
                        break;
                    case StatType.AttackRange:
                        filter.playerStats->AttackRange += upgrade.UpgradeValue;
                        break;
                }

                f.Events.MainPlayerStatsUpdated(filter.playerLink->Player, filter.playerStats->MovementSpeed, filter.playerStats->Damage, filter.playerStats->AttackRange);
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* playerLink;
            public PlayerStats* playerStats;
        }
    }
}

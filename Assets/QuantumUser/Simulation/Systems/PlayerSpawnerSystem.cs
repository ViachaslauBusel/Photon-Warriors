namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerSpawnerSystem : SystemSignalsOnly, ISignalOnPlayerAdded, ISignalOnPlayerRemoved
    {
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var runtimePlayer = f.GetPlayerData(player);
            var entity = f.Create(runtimePlayer.PlayerAvatar);
            var link = new PlayerLink { Player = player };
            f.Add(entity, link);

            if (f.Unsafe.TryGetPointer(entity, out Transform3D* transform))
            {
                transform->Position = new FPVector3(player * 2, 1, 0);
            }
        }

        public void OnPlayerRemoved(Frame f, PlayerRef player)
        {
            var filter = f.Filter<PlayerLink>();

            while (filter.Next(out var entity, out var link))
            {
                if (link.Player == player)
                {
                    f.Destroy(entity);
                }
            }
        }
    }
}

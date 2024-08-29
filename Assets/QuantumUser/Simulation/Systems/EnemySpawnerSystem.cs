using Photon.Deterministic;
using Quantum.Collections;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public class EnemySpawnerSystem : SystemMainThread
    {
        private EnemySpawnerConfig _config;

        public unsafe override void OnInit(Frame f)
        {
            f.Filter<EnemySpawnerConfig>().Next(out _, out _config);

            for (int i = 0; i < _config.MaxEnemies; i++)
            {
                SpawnEnemy(f);
            }
        }

        private unsafe void SpawnEnemy(Frame f)
        {
            var entity = f.Create(GetRandomProtoptype(f, _config.EnemyPrototypes));

            // Set random position
            if (f.Unsafe.TryGetPointer(entity, out Transform3D* transform))
            {
                transform->Position = new FPVector3(f.Global->RngSession.Next(-_config.ArenaSize, _config.ArenaSize),
                                                    1,
                                                    f.Global->RngSession.Next(-_config.ArenaSize, _config.ArenaSize));
            }
            // Set original health
            if (f.Unsafe.TryGetPointer(entity, out EntityHealth* health))
            {
                health->Health = health->MaxHealth;
            }
        }

        private unsafe AssetRef<EntityPrototype> GetRandomProtoptype(Frame f, QListPtr<AssetRef<EntityPrototype>> enemyPrototypes)
        {
            var list = f.ResolveList(enemyPrototypes);
            return list[f.Global->RngSession.Next(0, list.Count)];
        }

        public unsafe override void Update(Frame f)
        {
            TrySpawnEnemy(f);
        }

        private unsafe void TrySpawnEnemy(Frame f)
        {
            int enemyCount = f.ComponentCount<EntityHealth>();
            if (enemyCount <= _config.MaxEnemies)
            {
                SpawnEnemy(f);
            }
        }
    }
}

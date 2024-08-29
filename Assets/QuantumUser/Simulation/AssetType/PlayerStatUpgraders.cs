using Photon.Deterministic;
using Quantum.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quantum
{
    public class PlayerStatUpgraders : AssetObject
    {
        public List<StatUpgrader> StatUpgraders;

        public FP GetBaseValue(StatType statType)
        {
            var upgrader = StatUpgraders.FirstOrDefault(x => x.StatType == statType);
            return upgrader.BaseValue;
        }

        private FP SumUpgradeChances()
        {
            FP total = 0;
            foreach (var upgrader in StatUpgraders)
            {
                total += upgrader.UpgradeChance;
            }
            return total;
        }

        public unsafe StatUpgrader GetRandomStatUpgrader(Frame frame)
        {
            FP totalChance = SumUpgradeChances();
            FP randomValue = frame.Global->RngSession.Next(0, totalChance);
            FP currentChance = 0;

            for (int i = 0; i < StatUpgraders.Count; i++)
            {
                var upgrader = StatUpgraders[i];
                currentChance += upgrader.UpgradeChance;

                if (randomValue <= currentChance)
                {
                    return upgrader;
                }
            }

            return StatUpgraders[StatUpgraders.Count - 1];
        }

        [System.Serializable]
        public struct StatUpgrader
        {
            public StatType StatType;
            public FP BaseValue;
            public FP UpgradeValue;
            public FP UpgradeChance;
        }
    }
}

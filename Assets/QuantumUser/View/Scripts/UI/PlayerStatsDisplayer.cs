using Quantum.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Quantum.UI
{
    public class PlayerStatsDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _statsTxt;
        private DispatcherSubscription _subscribtion;

        private void Awake()
        {
           _subscribtion = QuantumEvent.Subscribe(listener: this, handler: (EventMainPlayerStatsUpdated e) => OnStatsUpdated(e));
        }

        private void OnStatsUpdated(EventMainPlayerStatsUpdated callback)
        {
            _statsTxt.text = $"Movement Speed: {callback.MovementSpeed}\n";
            _statsTxt.text += $"Damage: {callback.Damage}\n";
            _statsTxt.text += $"Attack Range: {callback.AttackRange}\n";
        }

        public void UpgradeStats()
        {
            QuantumRunner.Default.Game.SendCommand(new Commands.UpgradePlayerStatsCommand());
        }

        private void OnDestroy()
        {
            QuantumEvent.Unsubscribe(_subscribtion);
        }
    }
}

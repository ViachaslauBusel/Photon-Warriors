using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Quantum.UI
{
    public class PlayerKillsDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _killsText;
        private DispatcherSubscription _subscribtion;

        private void Awake()
        {
            _subscribtion = QuantumEvent.Subscribe(this, (EventPlayerKillsUpdated e) => OnPlayerKillsUpdated(e));
        }

        private void OnPlayerKillsUpdated(EventPlayerKillsUpdated callback)
        {
            _killsText.text = $"Kills: {callback.KillsCounter}";
        }

        private void OnDestroy()
        {
            QuantumEvent.Unsubscribe(_subscribtion);
        }
    }
}

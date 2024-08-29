using Photon.Client.StructWrapping;
using Photon.Deterministic;
using Quantum.Context;
using UnityEngine;
using UnityEngine.UI;

namespace Quantum.UI
{
    public class HealthView : QuantumEntityViewComponent<CustomViewContext>
    {
        [SerializeField]
        private Image _healthBar;
        private DispatcherSubscription _subscribtion;

        private void Awake()
        {
            _subscribtion = QuantumEvent.Subscribe(listener: this, handler: (EventHealthUpdated e) => OnHealthUpdated(e));
        }

        private void Start()
        {
            var health = PredictedFrame.Get<EntityHealth>(EntityRef);
            UpdateHealthBar(health.Health, health.MaxHealth);
        }

        private void OnHealthUpdated(EventHealthUpdated e)
        {
            if(e.Entity == EntityRef)
            {
                UpdateHealthBar(e.CurrentHealth, e.MaxHealth);
            }
        }

        private void UpdateHealthBar(FP currentHealth, FP maxHealth)
        {
            _healthBar.fillAmount = (currentHealth / maxHealth).AsFloat;
        }

        private void OnDestroy()
        {
            QuantumEvent.Unsubscribe(_subscribtion);
        }
    }
}

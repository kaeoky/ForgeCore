using System;
using UnityEngine;
using UnityEngine.Events;

namespace ForgeCore.Entities.Health
{
    public class HealthSystem : MonoBehaviour
    {
        // Health
        public float maxHealth = 10f;
        public float health = 10f;

        [NonSerialized]
        public readonly UnityEvent OnHealthChanged = new();
        
        [NonSerialized]
        public readonly UnityEvent OnDeath = new();
        
        public void Damage(float damage)
        {
            if (damage <= 0)
                return;
            
            health -= damage;
            OnHealthChanged?.Invoke();
        }

        public void Heal(float heal)
        {
            if (heal <= 0)
                return;
            
            health += heal;
            OnHealthChanged?.Invoke();
        }

        private void Kill()
        {
            OnDeath?.Invoke();
            OnKill();
        }

        protected virtual void OnKill()
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

namespace Gameplay.Systems {
    public class SimpleHurtBox : MonoBehaviour {
        public float DamageAmount = 10f;

        void OnTriggerEnter(Collider other)
        {
            var damageable = other.GetComponent<IDamageable<DamageData>>();

            if (damageable != null)
            {
                DamageData hitData = new DamageData
                {
                    Amount = DamageAmount,
                    AttackerPosition = transform.position,
                    AttackDirection = (other.transform.position - transform.position).normalized
                };

                damageable.TakeDamage(hitData);
            }
        }
    }
}

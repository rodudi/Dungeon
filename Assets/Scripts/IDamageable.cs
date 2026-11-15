using UnityEngine;

namespace Gameplay.Systems {
    public enum DamageSource {
        Player,
        Enemy,
        Environment
    }

    public struct DamageData {
        public float Amount;
        public Vector3 AttackerPosition;
        public Vector3 AttackDirection;
        public DamageSource Source; 
    }

    public interface IDamageable<T> {
        void TakeDamage(T damageInfo);
    }
}
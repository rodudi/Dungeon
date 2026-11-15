using UnityEngine;
using UnityEngine.Events;
using Gameplay.Systems;
using Gameplay.Character; 

public class CharacterHabilities : MonoBehaviour {
    public UnityAction OnAttack;
    [SerializeField] BoxCollider attackHitBox;

    private Damageable _damageable;
    private CharacterMovement _characterMovement;
    private CharacterFaction _characterFaction;
    void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _characterMovement = GetComponent<CharacterMovement>();
        _characterFaction = GetComponent<CharacterFaction>();
    }

    public void Attack()
    {
        if (!_damageable.IsDead)
        {
            OnAttack?.Invoke();
        }
    }

    public void HurtDamageables()
    {
        Collider[] colliders = Physics.OverlapBox(attackHitBox.bounds.center, attackHitBox.bounds.extents, attackHitBox.transform.rotation);

        foreach (Collider other in colliders)
        {
            var targetDamageable = other.GetComponent<IDamageable<DamageData>>();
            if (targetDamageable != null)
            {
                Vector3 attackDir = _characterMovement != null ? _characterMovement.LookDirection : transform.forward;
                DamageSource currentSource; 
                if (_characterFaction != null) 
                {
                    currentSource = _characterFaction.Faction;
                }
                else 
                {
                    currentSource = DamageSource.Environment;

                }

                DamageData hitData = new DamageData
                {
                    Amount = 10f,
                    AttackerPosition = transform.position,
                    AttackDirection = attackDir,
                    Source = _characterFaction.Faction
                };

                targetDamageable.TakeDamage(hitData);
            }
        }
    }
}
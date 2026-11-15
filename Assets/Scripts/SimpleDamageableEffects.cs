using UnityEngine;
using Gameplay.Systems;

public class SimpleDamageableEffects : MonoBehaviour
{
    Damageable _damageable;
    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _damageable.OnDamageTaken += TakeDamage;
        _damageable.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
    private void TakeDamage(float damage)
    {
        _animator.SetTrigger("damaged");
    }
}

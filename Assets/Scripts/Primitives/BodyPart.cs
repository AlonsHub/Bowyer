using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    //[SerializeField]
    public LivingBody livingBody;
    [SerializeField]
    protected float maxHealth;
    protected float _currentHealth;

    public bool IsDead => _currentHealth <= 0;

    public float GetMaxHealth { get => maxHealth; }

    protected virtual void Awake()
    {
        _currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        livingBody.ProcessDamage(damage);
        if (_currentHealth <= 0)
            Die();
    }

    public virtual void Die()
    {
        Debug.Log($"{name} is dead");
    }
}

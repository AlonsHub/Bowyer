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
    private void OnEnable()
    {
        livingBody.OnDeath += Stop;
    }
    private void OnDisable()
    {
        livingBody.OnDeath -= Stop;
    }

    public virtual void TakeDamage(float damage)
    {
        if(livingBody.IsDead)
            return;
        
        _currentHealth -= damage;
        livingBody.ProcessDamage(damage);
        if (_currentHealth <= 0)
            Die();
    }

    public virtual void Die()
    {
        Stop();
        Debug.Log($"{name} is dead");
    }

    public virtual void Stop()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingBody : MonoBehaviour
{
    [SerializeField]
    float _totalHealth;
    [SerializeField]
    float _currentHealth;
    [SerializeField]
    BodyPart[] bodyParts;
    [SerializeField]
    Rigidbody rb;

    public System.Action<float> OnAnyDamage;
    public System.Action OnDeath;

    protected virtual void Start()
    {
        _totalHealth = 0;
        foreach (var item in bodyParts)
        {
            _totalHealth += item.GetMaxHealth;
        }
        _currentHealth = _totalHealth;

    }
   
    public virtual void ProcessDamage(float damage)
    {
        _currentHealth -= damage;
        OnAnyDamage?.Invoke(damage);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log($"{name} is dead");
        //rb.isKinematic = false;

        foreach (var item in bodyParts)
        {
            if (item.IsDead)
                continue;

            item.Die();
        }

        gameObject.AddComponent<Rigidbody>();
        OnDeath?.Invoke();
    }
}

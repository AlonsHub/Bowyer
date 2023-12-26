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

    [SerializeField]
    protected bool isCriticalSpot;

    [SerializeField]
    Renderer rend;
    [SerializeField]
    float redTime;

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
        DamageFeedback();

        if (livingBody.IsDead)
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

    public void DamageFeedback()
    {
        StartCoroutine(ColorChange());
        if(isCriticalSpot)
        Temp_KeyMapper.Instance.CallCrithair();
            else
        Temp_KeyMapper.Instance.CallHithair();
    }

    IEnumerator ColorChange()
    {
        rend.material.SetColor("_BaseColor", Color.red);
        yield return new WaitForSeconds(redTime);
        rend.material.SetColor("_BaseColor", Color.white);
    }

    public virtual void Stop()
    {

    }
}

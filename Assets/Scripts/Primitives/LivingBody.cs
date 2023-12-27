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

    [SerializeField]
    Renderer rend;
    [SerializeField]
    float redTime;

    public bool IsDead => _currentHealth <= 0;

    public System.Action<float> OnAnyDamage;
    public System.Action OnDeath;


    //public System.Action<DeathData> OnDeath;

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
        //DamageFeedback();


        if (IsDead)
        {
            Temp_KeyMapper.Instance.CallKillhair();

            Die();
        }
    }


    public virtual void Die()
    {
        Debug.Log($"{name} is dead");
        //rb.isKinematic = false;

        //foreach (var item in bodyParts)
        //{
        //    if (item.IsDead)
        //        continue;

        //    item.Die();
        //}

        OnDeath?.Invoke();
    }
}
//[System.Serializable]
//public class DeathData
//{
//    public Vector3 killingBlowVector;
//}

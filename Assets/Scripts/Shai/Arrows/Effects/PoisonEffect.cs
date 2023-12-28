using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : ArrowEffect
{
    private float cycleTime, cycleDamage;

    public PoisonEffect(LivingBody _livingBody, float _effectTime, float _cycleTime, float _cycleDamage) : base(_livingBody)
    {
        timer = _effectTime;
        cycleTime = _cycleTime;
        cycleDamage = _cycleDamage;
    }

    private void Start()
    {
        StartCoroutine(HitCycle());
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(this);
        }
    }

    private IEnumerator HitCycle()
    {
        while (true)
        {
            livingBody.ProcessDamage(cycleDamage);
            yield return new WaitForSeconds(cycleTime);
        }
    }


    public override void ApplyStatusEffect()
    {
        throw new System.NotImplementedException();
    }
}

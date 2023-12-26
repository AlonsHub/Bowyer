using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArrowEffect : MonoBehaviour
{
    protected LivingBody livingBody;


    public ArrowEffect(LivingBody _livingBody)
    {
        livingBody = _livingBody;
    }

    public abstract void ApplyStatusEffect();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArrowEffect : MonoBehaviour
{
    [SerializeField] private AudioClip releaseSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip enemyHitSound;

    public virtual void OnHit(Collider other)
    {
        //if collider is enemy play enemyHitSound
        //otherwise play hitSound
        
    }

    public virtual void OnShoot()
    {
        //play releaseSound
    }
}

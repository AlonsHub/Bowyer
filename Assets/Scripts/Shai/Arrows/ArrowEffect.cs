using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class ArrowEffect : MonoBehaviour
{
    [SerializeField] private AudioSource releaseSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource enemyHitSound;

    public abstract void OnHit(Collider other);

    public abstract void OnShoot();
}

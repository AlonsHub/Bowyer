using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_LegDelay : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] Animator anim;

    private void Awake()
    {
        anim.enabled = false;
        StartCoroutine(nameof(DelayEnable));
    }

    IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(delay);
        anim.enabled = true;
    }
}

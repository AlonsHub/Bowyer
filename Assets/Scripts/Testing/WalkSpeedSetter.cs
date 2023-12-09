using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpeedSetter : MonoBehaviour
{
    [SerializeField] Animator[] anims;
    [SerializeField] float animModifier;
    
    float speed;
    public void SetSpeed(float newSpeed)
    {
        foreach (var anim in anims)
        {
            if(anim.enabled)
            anim.speed = newSpeed * animModifier;
        }
        speed = newSpeed;
    }
}

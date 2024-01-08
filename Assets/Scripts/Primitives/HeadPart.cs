using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPart : BodyPart
{
    public override void Die()
    {
        if(livingBody.IsDead)   
            return;
        
        _currentHealth = 0;
        livingBody.Die();
    }
}

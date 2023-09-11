using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPart : BodyPart
{
    public override void Die()
    {
        livingBody.Die();
    }
}

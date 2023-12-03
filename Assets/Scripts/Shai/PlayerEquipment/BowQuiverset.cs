using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BowQuiverset 
{
    public BowShai bow;
    public Quiver quiver;

    public BowShai Bow { get { return bow; } }
    public Quiver Quiver { get { return quiver; } }

    public void ChangeBow(BowShai newBow)
    {
        bow = newBow;
    }

    public void ChangeQuiver(Quiver newQuiver)
    {
        quiver = newQuiver;
    }
}

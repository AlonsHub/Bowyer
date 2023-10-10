using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfBowPart {Arm, Grip, String};

[System.Serializable]
public class BowPart 
{
    /// <summary>
    /// In units of force, the "tension" at which this part would break?
    /// </summary>
    public float breakingPoint;
    public float weight;

}

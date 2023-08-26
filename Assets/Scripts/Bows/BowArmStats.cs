using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BowArmStats : BowPart
{
    //TBD Consider making a tension-gradient instead of all these floats

    /// <summary>
    /// Maximum force 
    /// </summary>
    [Tooltip("Maximum force")]
    public float maxTension;
    /// <summary>
    /// The range of forces to be in, for a perfect shot
    /// </summary>
    [Tooltip("The range of forces to be in, for a perfect shot")]
    public T2<float> perfectTension;
    /// <summary>
    /// Below this force, shots will not be fired
    /// </summary>
    [Tooltip("Below this force, shots will not be fired")]
    public float minTension;


    /// <summary>
    /// At the moment, only effects pullTime
    /// </summary>
    public float resistence;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BowStringStats : BowPart
{
    /// <summary>
    /// Makes the shot weaker - lower value for stronger shots
    /// </summary>
    [Tooltip("Makes the shot weaker - lower value for stronger shots")]
    public float elasticity;
    /// <summary>
    /// Determins how far the string can pull back.
    /// At the moment, this ONLY affects the final postion of the arrow when pulled
    /// </summary>
    [Tooltip("Determins how far the string can pull back. At the moment, this ONLY affects the final postion of the arrow when pulled")]
    public float stringLength;
    /// <summary>
    /// Makes the shot louder per unit of force
    /// </summary>
    [Tooltip("Makes the shot louder per unit of force")]
    public float noisePerUnitOfForce;
}

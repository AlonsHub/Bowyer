using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BowStats 
{
    public float PullResistence => armStats.resistence;
    public float MaxPull_Tension => armStats.maxTension;
    public float MaxPull_ArrowDistance => stringStats.stringLength;
    public float PullFactor => stringStats.elasticity == 0? 0: 1/stringStats.elasticity;

    public bool IsPerfect(float force) => force >= armStats.perfectTension.x && force <= armStats.perfectTension.y;

    //array of parts?
    //speicific slots for parts?
    //parts with sub parts
    public BowStringStats stringStats;
    public BowArmStats armStats;
}

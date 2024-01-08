using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LegPart : BodyPart
{
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    float speedToLose;
    [SerializeField]
    Animator anim;
    public override void Stop()
    {
        agent.speed -= speedToLose;
        anim.enabled = false;
    }
}

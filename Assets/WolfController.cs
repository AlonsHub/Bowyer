using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent; 
    [SerializeField] WalkSpeedSetter speedSetter;

    [SerializeField] float randomRadius;

    void Start()
    {
        StartCoroutine(nameof(Wander));
    }

    IEnumerator Wander()
    {
        while (true)
        {
            SetRandomDestinaion();
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);

            yield return new WaitForSeconds(Random.Range(0f, 2f));
        }
    }

    void SetRandomDestinaion()
    {
        Vector3 random = Random.insideUnitSphere * randomRadius;
        random.y = 0;
        random += transform.position;

        SetDestination(random);
    }

    void SetDestination(Vector3 newDestination)
    {
        agent.SetDestination(newDestination);
    }

    private void FixedUpdate()
    {
        speedSetter.SetSpeed(agent.velocity.magnitude);
    }

}

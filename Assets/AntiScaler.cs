using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiScaler : MonoBehaviour
{
    [SerializeField]
    Transform parentScaler;

    private void LateUpdate()
    {
        transform.localScale = new Vector3(transform.localScale.x, 1f / parentScaler.localScale.y, transform.localScale.z);
    }
}

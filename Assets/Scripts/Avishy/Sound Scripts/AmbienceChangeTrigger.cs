using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    [Header("Parameter Data")]
    [SerializeField] private string paramName;
    [SerializeField] private float paramValue;
    [SerializeField] private MusicZones zone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.SetAmbienceParameter(paramName, paramValue);
            AudioManager.Instance.SetMusicArea(zone);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;
    [SerializeField]
    GameObject toggleAll;
    [SerializeField]
    UnityEngine.UI.Toggle muteToggle;

    bool isOpen = false;
  

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = !isOpen;
            toggleAll.SetActive(isOpen);
            PlayerController.ActionInputPanelsEnabled = !isOpen;

            Cursor.lockState = isOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = isOpen;
        }
    }

    public void SetMute()
    {
        audioManager.SetMute(muteToggle.isOn);
    }
}

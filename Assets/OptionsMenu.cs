using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    GameObject toggleAll;

    bool isOpen = false;
  

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = !isOpen;
            toggleAll.SetActive(isOpen);

            Cursor.lockState = isOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
        }
    }
}

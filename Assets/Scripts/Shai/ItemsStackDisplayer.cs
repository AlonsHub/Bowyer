using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ItemsStackDisplayer : MonoBehaviour
{
    [SerializeField] private ItemHolderData holderData;
    [SerializeField] private TMP_Text stackText;
    [SerializeField] private Transform stackTextPos;
    [SerializeField] private Transform itemPos;
    [SerializeField] private Vector3 hoverOffest;

    private Transform playerCam;
    private bool displayStackNum;


    private void Awake()
    {
        playerCam = Camera.main.transform;
        ToggleActive(holderData.ReturnItemSO().IsStackable);
    }

    private void Start()
    {
        UpdateNumText();
    }

    private void OnEnable()
    {
        UpdateNumText();
    }

    private void LateUpdate()
    {
        FixTextPos();
    }

    private void FixTextPos()
    {
        if (displayStackNum)
        {
            stackTextPos.position = itemPos.position + hoverOffest;
            stackTextPos.rotation = Quaternion.LookRotation(stackTextPos.position - playerCam.position);
        }
    }

    private void UpdateNumText()
    {
        stackText.text = "x" + holderData.Stack.ToString();
    }

    public void ToggleActive(bool activeState)
    {
        displayStackNum = activeState;
        stackText.gameObject.SetActive(activeState);
    }

    public void SetLookAtPos(Transform newPos)
    {
        //we might want to use this if and when we change the player's camera
        playerCam = newPos;
    }

}

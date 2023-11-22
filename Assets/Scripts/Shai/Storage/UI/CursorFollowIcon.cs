using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CursorFollowIcon : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountTxt;
    [SerializeField] private Vector3 offSet;

    private void LateUpdate()
    {
        if (true)
        {

        }
        Follow();
    }

    private void Follow()
    {
        transform.position = Input.mousePosition + offSet;
    }

    public void SetDisplay(InventorySlotUI slotUI)
    {
        if (slotUI != null)
        {
            icon.gameObject.SetActive(true);

            icon.sprite = slotUI.Slot.Item.ReturnItemSO().Icon;
            if (slotUI.Slot.Item.ReturnItemSO().IsStackable)
            {
                amountTxt.gameObject.SetActive(true);
                amountTxt.text = slotUI.Slot.Item.Stack.ToString();
            }
            else
            {
                amountTxt.gameObject.SetActive(false);
            }
        }
        else
        {
            icon.gameObject.SetActive(false);
            amountTxt.gameObject.SetActive(false);
        }
    }

}

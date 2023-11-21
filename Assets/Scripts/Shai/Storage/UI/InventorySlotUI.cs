using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image greyMask;
    [SerializeField] private TMP_Text stackNumTxt;
    [SerializeField] private InventorySlot slot;

    public Sprite ItemIcon { get { return itemIcon.sprite; } }
    public InventorySlot Slot { get { return slot; } }

    public void OnPointerDown(PointerEventData eventData)
    {
        BlackBoard.Instance.ItemManager.RecieveSlot(this);
    }

   

    public void SetItemIcon(Sprite icon)
    {
        if (icon == null)
        {
            itemIcon.gameObject.SetActive(false);
        }
        else
        {
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = icon;
        }
    }

    public void SetStackText(bool isStackable, int currentNum, int maxNum)
    {
        if (isStackable)
        {
            stackNumTxt.gameObject.SetActive(true);
            string stackTxt = currentNum.ToString() + "/" + maxNum.ToString();
            stackNumTxt.text = stackTxt;
        }
        else
        {
            stackNumTxt.gameObject.SetActive(false);
        }
    }

    public void ToggleGreyMask(bool activeState)
    {
        greyMask.gameObject.SetActive(activeState);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour
{
    [SerializeField] private CursorFollowIcon cursorFollowIcon;
    private InventorySlotUI currentSlot;


    public void RecieveSlot(InventorySlotUI slotUI)
    {
        if (currentSlot == null)
        {
            SetCurrentSlot(slotUI);
        }
        else
        {
            MoveItem(currentSlot, slotUI);
        }
    }

    private void SetCurrentSlot(InventorySlotUI slotUI)
    {
        if (slotUI.Slot.Item.ReturnItemSO() != null)
        {
            currentSlot = slotUI;
            slotUI.ToggleGreyMask(true);
            UpdateCursorIcon();
        }
    }

    private void ClearSlot()
    {
        currentSlot.ToggleGreyMask(false);
        UpdateCursorIcon();
        currentSlot = null;
    }

    private void MoveItem(InventorySlotUI fromSlotUI, InventorySlotUI toSlotUI)
    {
        if (toSlotUI.Slot.Item.ReturnItemSO() == null)//targeted slot is empty (add)
        {
            toSlotUI.Slot.AddItem(fromSlotUI.Slot.Item);
            fromSlotUI.Slot.RemoveItem();
            toSlotUI.Slot.UpdateUI();
            fromSlotUI.Slot.UpdateUI();
            ClearSlot();
        }
        else if (fromSlotUI.Slot.Item.ReturnItemSO() == toSlotUI.Slot.Item.ReturnItemSO())//both slots have the same item (add)
        {
            int excess = toSlotUI.Slot.AddItem(fromSlotUI.Slot.Item);
            fromSlotUI.Slot.Item.SetStack(excess);
            toSlotUI.Slot.UpdateUI();
            fromSlotUI.Slot.UpdateUI();
            if (excess == 0)
            {
                ClearSlot();
            }
            else
            {
                UpdateCursorIcon();
            }
        }
        else if (fromSlotUI.Slot.Item.ReturnItemSO() != toSlotUI.Slot.Item.ReturnItemSO())//each slot contains different item (switch)
        {

        }
    }

    private void UpdateCursorIcon()
    {
        cursorFollowIcon.SetDisplay(currentSlot.Slot.Item);
    }
}

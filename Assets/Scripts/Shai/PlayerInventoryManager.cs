using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private Transform itemDropPos;
    [SerializeField] private CursorFollowIcon cursorFollowIcon;
    [SerializeField] private GameObject playerInventoryUI;
    [SerializeField] private List<SmoothRotator> playerRotators;
    
    private InventorySlotUI currentSlot;
    private bool isActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isActive = !isActive;
            playerInventoryUI.SetActive(!playerInventoryUI.activeSelf);
            Cursor.lockState = playerInventoryUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            foreach (var rotator in playerRotators)
            {
                rotator.enabled = !rotator.enabled;
            }

            if (!isActive)//reset slots
            {
                ClearCurrentSlot();
            }

        }
    }


    public void RecieveSlot(InventorySlotUI slotUI)
    {
        if (currentSlot == null && slotUI != null)
        {
            SetCurrentSlot(slotUI);
        }
        else if (slotUI != null)
        {
            MoveItem(currentSlot, slotUI);
        }
        else if (slotUI == null && currentSlot != null)
        {
            DropItem();
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

    private void ClearCurrentSlot()
    {
        currentSlot?.ToggleGreyMask(false);
        currentSlot = null;
        UpdateCursorIcon();
    }

    private void MoveItem(InventorySlotUI fromSlotUI, InventorySlotUI toSlotUI)
    {
        if (toSlotUI.Slot.Item.ReturnItemSO() == null)//targeted slot is empty (add)
        {
            toSlotUI.Slot.AddItem(fromSlotUI.Slot.Item);
            fromSlotUI.Slot.RemoveItem();
            toSlotUI.Slot.UpdateUI();
            fromSlotUI.Slot.UpdateUI();
            ClearCurrentSlot();
        }
        else if (fromSlotUI.Slot.Item.ReturnItemSO() == toSlotUI.Slot.Item.ReturnItemSO())//both slots have the same item (add)
        {
            int excess = toSlotUI.Slot.AddItem(fromSlotUI.Slot.Item);
            fromSlotUI.Slot.Item.SetStack(excess);
            toSlotUI.Slot.UpdateUI();
            fromSlotUI.Slot.UpdateUI();
            if (excess == 0)
            {
                ClearCurrentSlot();
            }
            else
            {
                UpdateCursorIcon();
            }
        }
        else if (fromSlotUI.Slot.Item.ReturnItemSO() != toSlotUI.Slot.Item.ReturnItemSO())//each slot contains different item (switch)
        {
            ItemHolderData tmpItem;
            tmpItem = fromSlotUI.Slot.Item;

            ItemSO tmpItemSO = fromSlotUI.Slot.Item.ReturnItemSO();
            int tmpStack = fromSlotUI.Slot.Item.Stack;

            fromSlotUI.Slot.RemoveItem();
            fromSlotUI.Slot.AddItem(toSlotUI.Slot.Item);

            toSlotUI.Slot.RemoveItem();
            toSlotUI.Slot.Item.SetStack(tmpStack);
            toSlotUI.Slot.Item.AssignItemSO(tmpItemSO);

            toSlotUI.Slot.UpdateUI();
            fromSlotUI.Slot.UpdateUI();
            ClearCurrentSlot();
        }
    }

    public void DropItem()
    {
        GameObject droppedItemGO = Instantiate(currentSlot.Slot.Item.ReturnItemSO().ItemPrefab, itemDropPos.position, itemDropPos.rotation);
        ItemHolderData itemData;
        droppedItemGO.TryGetComponent<ItemHolderData>(out itemData);

        itemData.SetStack(currentSlot.Slot.Item.Stack);

        currentSlot.Slot.RemoveItem();
        currentSlot.Slot.UpdateUI();
        ClearCurrentSlot();

    }

    private void UpdateCursorIcon()
    {
        cursorFollowIcon.SetDisplay(currentSlot);
    }
}

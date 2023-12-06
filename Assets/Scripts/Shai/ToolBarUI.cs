using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToolBarUI : MonoBehaviour
{
    [SerializeField] private GameObject slotHighlight;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private List<InventorySlot> slots;
    private int currentSlotIndex;
    
    public void AddSlots(List<InventorySlot> slotsToAdd)
    {
        slots.AddRange(slotsToAdd);
        foreach (var slot in slotsToAdd)
        {
            slot.transform.SetParent(gridLayout.transform);
        }
    }

    public void ChangeCurrentSlotByStep(int step)
    {
        currentSlotIndex += step;
        if (currentSlotIndex < 0) { currentSlotIndex = slots.Count - 1; }
        else if (currentSlotIndex >= slots.Count) { currentSlotIndex = 0; }
        
        slotHighlight.transform.position = slots[currentSlotIndex].transform.position;
    }

    public void ChangeCurrentSlotByIndex(int index)
    {
        currentSlotIndex = index;
        slotHighlight.transform.position = slots[currentSlotIndex].transform.position;
    }

    public ItemHolderData GetCurrentItem()
    {
        if (slots[currentSlotIndex].IsEmpty)
        {
            return null;
        }
        return slots[currentSlotIndex].Item;
    }

    public ItemHolderData RemoveCurrentItem()
    {
        if (slots[currentSlotIndex].IsEmpty)
        {
            return null;
        }
        ItemHolderData IHD = slots[currentSlotIndex].Item;
        slots[currentSlotIndex].RemoveItem();
        return IHD;
    }
}

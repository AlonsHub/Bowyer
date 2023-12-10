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
        int tmpIndex = GetTmpIndexWithStep(step);

        if (slots[tmpIndex].IsEmpty)
        {
            //disable a slot's GO if empty and enabled (might be irrelevant once bar refresh is implemented)
            if (slots[tmpIndex].gameObject.activeSelf)
            {
                slots[tmpIndex].gameObject.SetActive(false);
            }

            //check if all of the slots are empty and abort if so
            bool empty = true;
            foreach (var slot in slots)
            {
                if (slot.IsEmpty)
                {
                    empty = false;
                }
            }
            if (empty)
            {
                return;
            }


        }


        currentSlotIndex = GetTmpIndexWithStep(step);

        slotHighlight.transform.position = slots[currentSlotIndex].transform.position;
    }

    private int GetTmpIndexWithStep(int step)
    {
        int tmpCurrentIndex = currentSlotIndex;
        tmpCurrentIndex += step;
        if (tmpCurrentIndex < 0) { tmpCurrentIndex = slots.Count - 1; }
        else if (tmpCurrentIndex >= slots.Count) { tmpCurrentIndex = 0; }

        return tmpCurrentIndex;
    }

    // need to chage this, what happens if the player presses 3 when slot3 is disabled but slot4 appears as 3 hmmm?
    public void ChangeCurrentSlotByIndex(int index) 
    {
        if (!slots[index].IsEmpty)
        {
            currentSlotIndex = index;
            slotHighlight.transform.position = slots[currentSlotIndex].transform.position;
        }
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

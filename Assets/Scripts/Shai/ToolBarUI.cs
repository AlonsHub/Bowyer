using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToolBarUI : MonoBehaviour
{
    [SerializeField] private GameObject slotHighlight;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private List<InventorySlot> slots;
    [SerializeField] private BowsLogic bowsLogic;
    private int currentSlotIndex;

    private void OnEnable()
    {
        BowsLogic.OnEquipQuiver += AddSlots;
        Invoke(nameof(RefreshToolbar), 0.1f);
    }

    private void OnDisable()
    {
        BowsLogic.OnEquipQuiver -= AddSlots;
    }

    public void AddSlots(List<InventorySlot> slotsToAdd)
    {
        //remove slots?
        if (slots != null && slots.Count > 0)
            ClearSlots();

        slots.AddRange(slotsToAdd);
        foreach (var slot in slotsToAdd)
        {
            slot.transform.SetParent(gridLayout.transform);
        }

        RefreshToolbar();
    }

    void ClearSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].transform.SetParent(null);
            slots[i].transform.position = new Vector3(0,-10000,0);
        }
        slots.Clear();
    }

    public void ChangeCurrentSlotByStep(int step)
    {
        int tmpIndex = GetTmpIndexWithStep(step);

        if (slots[tmpIndex].IsEmpty)
        {

            //check if all of the slots are empty and abort if so
            bool barEmpty = true;
            foreach (var slot in slots)
            {
                if (slot.IsEmpty)
                {
                    barEmpty = false;
                }
            }
            if (barEmpty)
            {
                return;
            }
            else //recurse attempt,still not working as it should. single scroll not removing all empty slots
            {
                currentSlotIndex = tmpIndex;
                ChangeCurrentSlotByStep(step);
            }
        }
        else
        {
            currentSlotIndex = tmpIndex;

            slotHighlight.transform.position = slots[currentSlotIndex].transform.position;
        }
        SyncQuiverIndex(); 
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
            SyncQuiverIndex();
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

    private void SyncQuiverIndex()
    {
        bowsLogic.ChangeCurrentArrowIndexByindex(currentSlotIndex);
    }

    [ContextMenu("Refresh")]
    private void RefreshToolbar()
    {
        //disable any empty slots and enable occupied ones who are disabled
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.gameObject.SetActive(false);
            }
            else if (!slot.gameObject.activeSelf)
            {
                slot.gameObject.SetActive(true);
            }
        }

        //try to fix the position and status of the highlighter,
        //seems to work but on the start of the game the highlighter isnt in the right position
        if (slots.Count > 0 && !slots[currentSlotIndex].IsEmpty)
        {
            slotHighlight.SetActive(true);
            slotHighlight.transform.position = slots[currentSlotIndex].transform.position;
        }
        else
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty)
                {
                    slotHighlight.SetActive(true);
                    slotHighlight.transform.position = slot.transform.position;
                    return;
                }
            }
            slotHighlight.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//- "slots" list in BaseInventory serves as the default arrows list
//- to add normal arrows to the quiver use the AddItem method from baseInventory
//- might want to override additem to be able to add both special and normal arrows using the same method
public class Quiver : BaseInventory
{
    private int currentArrowIndex;
    private List<InventorySlot> sArrowsSlots = new List<InventorySlot>();

    public ArrowSO GetArrowAt(int index)
    {
        return (ArrowSO)slots[index].Item.ReturnItemSO();
    }

    public void RemoveArrowAt(int index)
    {
        slots[index].Item.RemoveAmount(1);//remove one arrow

        if (slots[index].IsEmpty)//if no moew arrows are left in slot, orginize it 
        {
            for (int i = index; i < slots.Count - 1; i++)
            {
                slots[i].AddItem(slots[i + 1].RemoveItem());
            }
        }
        OnInventoryChanged.Invoke();

    }

    public ArrowSO GetCurrentArrow()
    {
        //check if empty first?

        return (ArrowSO)slots[currentArrowIndex].Item.ReturnItemSO();
    }

    public void ChangeCurrentArrowIndexByStep(int step)
    {
        int newIndex = currentArrowIndex += step;
        currentArrowIndex = Mathf.Clamp(newIndex, 0, slots.Count);
    }

    public void ChangeCurrentArrowIndexByNum(int newIndex)
    {
        currentArrowIndex = Mathf.Clamp(newIndex, 0, slots.Count);
    }

    /// <summary>
    /// Try to add a special arrow to special arrows slots in quiver and returns true if succefull
    /// </summary>
    /// <param name="arrowIHD"></param>
    /// <returns></returns>
    public bool AddSpecialArrow(ItemHolderData arrowIHD)
    {
        //look for empty slot
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(arrowIHD);
                return true;
            }
        }
        return false;
    }

    public void ImproveQuiver(int normalSlotsToAdd, int specialSlotsToAdd) 
    {
        //update lists here, i didnt need to implement, it so you do it! 

        OnInventoryChanged.Invoke();
    }

    public override List<InventorySlot> GetAllSlots()
    {
        List<InventorySlot> allArrows = new List<InventorySlot>();
        allArrows.AddRange(slots);
        allArrows.AddRange(sArrowsSlots);
        return allArrows;
    }
}

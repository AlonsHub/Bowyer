using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//- "slots" list in BaseInventory serves as the default arrows list
//- to add normal arrows to the quiver use the AddItem method from baseInventory
//- might want to override additem to be able to add both special and normal arrows using the same method
public class Quiver : BaseInventory
{
    [SerializeField] private QuiverSO so;
    [SerializeField] private List<InventorySlot> sArrowsSlots = new List<InventorySlot>();
    private int currentArrowIndex;

    public QuiverSO SO { get { return so; } }

    public ArrowSO GetArrowAt(int index)
    {
        if (index <= slots.Count - 1)
        {
            return (ArrowSO)slots[index].Item.ReturnItemSO();
        }

        int newIndex = index - slots.Count - 1;
        if (newIndex <= sArrowsSlots.Count - 1 && newIndex >= 0)
        {
            return (ArrowSO)sArrowsSlots[index].Item.ReturnItemSO();
        }

        Debug.Log("Invalid Index");
        return null;
    }
    public void RemoveCurrentArrow()
    {
        RemoveArrowAt(currentArrowIndex);
        //handle index reset if needed
    }
    public void RemoveArrowAt(int index)
    {
        if (index <= slots.Count - 1)//remove arrow from default arrows slots
        {
            slots[index].Item.RemoveAmount(1);//remove one arrow

            if (slots[index].IsEmpty)//if no more arrows are left in slot, orginize it 
            {
                for (int i = index; i < slots.Count - 1; i++)
                {
                    slots[i].AddItem(slots[i + 1].RemoveItem());
                }
            }
        }
        else if (index - slots.Count - 1 <= sArrowsSlots.Count - 1 && index - slots.Count - 1 >= 0)//remove arrow from special arrows slots
        {
            int newIndex = index - slots.Count - 1;
            sArrowsSlots[newIndex].Item.RemoveAmount(1);

            if (sArrowsSlots[newIndex].IsEmpty)
            {
                for (int i = newIndex; i < sArrowsSlots.Count - 1; i++)
                {
                    sArrowsSlots[i].AddItem(slots[i + 1].RemoveItem());
                }
            }
        }
        else
        {
            Debug.Log("Invalid Index");
        }

        //slots[index].Item.RemoveAmount(1);//remove one arrow

        //if (slots[index].IsEmpty)//if no moew arrows are left in slot, orginize it 
        //{
        //    for (int i = index; i < slots.Count - 1; i++)
        //    {
        //        slots[i].AddItem(slots[i + 1].RemoveItem());
        //    }
        //}
        //OnInventoryChanged.Invoke();

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
    public int AddSpecialArrow(ItemHolderData arrowIHD)
    {

        int itemsNum = arrowIHD.Stack;

        if (containableItemTypes.HasFlag(arrowIHD.ReturnItemSO().Type))
        {
            InventorySlot slot = GetAvailableSpecialSlot(arrowIHD);
            while (itemsNum > 0 && slot != null)//keep adding items until stack reaches 0
            {
                itemsNum = slot.AddItem(arrowIHD);
                slot = GetAvailableSpecialSlot(arrowIHD);
            }
            OnInventoryChanged.Invoke();

        }
        else
        {
            Debug.Log(gameObject.name + " cannot contain item of type: " + arrowIHD.ReturnItemSO().Type);
        }

        return itemsNum;
    }


    private InventorySlot GetAvailableSpecialSlot(ItemHolderData item)
    {
        //try to find a slot with the same item
        foreach (var slot in sArrowsSlots)
        {
            if (slot.Item.ReturnItemSO() != null
                && slot.Item.ReturnItemSO().ID == item.ReturnItemSO().ID
                && slot.Item.ReturnItemSO().StackMax > slot.Item.Stack
                && slot.Item.ReturnItemSO().IsStackable)
            {
                return slot;
            }
        }

        //try to find empty slot
        foreach (var slot in sArrowsSlots)
        {
            if (slot.Item.ReturnItemSO() == null)
            {
                return slot;
            }
        }

        //coudln't find slot
        return null;
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

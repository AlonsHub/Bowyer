using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiver : BaseInventory
{
    
    public ArrowSO GetArrowAt(int index) 
    {
        return (ArrowSO)slots[index].Item.ReturnItemSO();
    }

    public void RemoveArrowAt(int index)
    {
        for (int i = index; i < slots.Count - 1; i++)
        {
            slots[i].RemoveItem();
            slots[i].AddItem(slots[i + 1].RemoveItem());
        }
    }
}

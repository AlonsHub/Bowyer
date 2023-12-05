using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiver : BaseInventory
{
    private int currentArrowIndex;

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
}

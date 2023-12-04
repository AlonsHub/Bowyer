using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolderData : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private int _stack;
    public int Stack { get { return _stack; } }


    public ItemSO ReturnItemSO()
    {
        return itemSO;
    }

    public bool AssignItemSO(ItemSO _itemSO)
    {
        if (itemSO == null)
        {
            itemSO = _itemSO;
            Debug.Log(itemSO.name + " assigned to " + this.name);
            return true;
        }
        else
        {
            Debug.Log(itemSO.name + " couldn't be assigned to " + this.name + " since it's occupied");
            return false;
        }
    }

    /// <summary>
    /// Adds amount of items to stack, returns excess items.
    /// </summary>
    public int AddAmount(int amountToAdd)
    {
        int excess = 0;
        _stack += amountToAdd;
        if (_stack > itemSO.StackMax)
        {
            excess = _stack - itemSO.StackMax;
            _stack = itemSO.StackMax;
        }
        return excess;
    }


    public int RemoveAmount(int amountToremove)
    {
        int removedAmount = amountToremove;
        if (amountToremove > _stack)
        {
            removedAmount = _stack;
            _stack = 0;
            Debug.Log("tried to remove more items than possible");
        }
        else
        {
            _stack -= amountToremove;
        }

        if (_stack == 0)
        {
            itemSO = null;
        }

        return removedAmount;
    }

    public void SetStack(int newStack)
    {
        _stack = newStack;
        if (_stack == 0)
        {
            itemSO = null;
        }
    }
}

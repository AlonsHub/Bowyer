using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemHolderData item;

    public ItemHolderData Item { get { return item; } }


    /// <summary>
    /// Add item to slot, return amount of an item that cant be added to the slot
    /// </summary>
    public int AddItem(ItemHolderData _item)
    {
        int leftOver = 0;
        if (item.ReturnItemSO() == null)
        {
            item.AssignItemSO(_item.ReturnItemSO());
            item.AddAmount(_item.Stack);
            Debug.Log("new " + _item.Stack.ToString() + _item.ReturnItemSO().Name + " added to slot");
        }
        else if (item.ReturnItemSO().ID == _item.ReturnItemSO().ID)
        {
            leftOver = item.AddAmount(_item.Stack);
            Debug.Log(_item.Stack.ToString() + _item.ReturnItemSO().Name + " added to slot");
        }
        else
        {
            //if item cant be added, return the entire stack
            leftOver = _item.Stack;
            Debug.Log(_item.Stack.ToString() + _item.ReturnItemSO().Name + " couldn't be added to slot");
        }
        return leftOver;
    }

    public ItemHolderData RemoveItem()
    {
        ItemHolderData tmpItem = item;
        item = null;
        return tmpItem;
    }


    // ***this method should probably move to PlayerItemManager***
    /// <summary>
    /// transfers as many items as possible from this slot to another, destroys itemHolder if stack reaches 0, ?<-(switches between the items if id's are different)
    /// </summary>
    public void TransferItem(InventorySlot slot)
    {
        if (item.ReturnItemSO().ID == slot.Item.ReturnItemSO().ID)
        {
            int leftOver = slot.AddItem(item);
            item.RemoveAmount(item.Stack);
            item.AddAmount(leftOver);
            if (item.Stack == 0)
            {
                //Destroy(item);
                item = null;
            }
        }
        else //not sure about this part (switch part)
        {
            ItemHolderData tmpItem = item;

        }

    }
}

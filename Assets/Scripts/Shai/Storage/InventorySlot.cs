using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemHolderData _item;
    [SerializeField] private InventorySlotUI _ui;
    public ItemHolderData Item { get { return _item; } }



    /// <summary>
    /// Add item to slot, return amount of an item that cant be added to the slot
    /// </summary>
    public int AddItem(ItemHolderData _item)
    {
        int leftOver = 0;
        if (this._item.ReturnItemSO() == null)
        {
            this._item.AssignItemSO(_item.ReturnItemSO());
            this._item.AddAmount(_item.Stack);
            Debug.Log("new " + _item.Stack.ToString() + _item.ReturnItemSO().Name + " added to slot");
        }
        else if (this._item.ReturnItemSO().ID == _item.ReturnItemSO().ID)
        {
            leftOver = this._item.AddAmount(_item.Stack);
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
        ItemHolderData tmpItem = _item;
        _item.RemoveAmount(_item.Stack);
        return tmpItem;
    }


    // ***this method should probably move to PlayerItemManager***
    /// <summary>
    /// transfers as many items as possible from this slot to another, destroys itemHolder if stack reaches 0, ?<-(switches between the items if id's are different)
    /// </summary>
    public void TransferItem(InventorySlot slot)
    {
        if (_item.ReturnItemSO().ID == slot.Item.ReturnItemSO().ID)
        {
            int leftOver = slot.AddItem(_item);
            _item.RemoveAmount(_item.Stack);
            _item.AddAmount(leftOver);
            if (_item.Stack == 0)
            {
                //Destroy(item);
                _item = null;
            }
        }
        else //not sure about this part (switch part)
        {
            ItemHolderData tmpItem = _item;

        }

    }

    public void UpdateUI()
    {
        if (Item.ReturnItemSO() != null)
        {
            _ui.SetItemIcon(_item.ReturnItemSO().Icon);
            _ui.SetStackText(_item.ReturnItemSO().IsStackable, _item.Stack, _item.ReturnItemSO().StackMax);
        }
        else
        {
            _ui.SetItemIcon(null);
        }
    }

}

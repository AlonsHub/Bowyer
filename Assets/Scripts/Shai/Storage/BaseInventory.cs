using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInventory : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int capacity;
    private List<InventorySlot> slots;



    private void Awake()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < capacity; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, transform);
            InventorySlot newSlot = null;
            if (slotGO.TryGetComponent<InventorySlot>(out newSlot))
            {
                slots.Add(newSlot);
            }
        }
    }

    public int AddItem(ItemHolderData item)
    {
        int itemsNum = item.Stack;
        InventorySlot slot = GetAvailableSlot(item);
        while (itemsNum > 0 && slot != null)
        {
            itemsNum = slot.AddItem(item);
            slot = GetAvailableSlot(item);
        }
        return itemsNum;
    }

    private InventorySlot GetAvailableSlot(ItemHolderData item)
    {
        //try to find a slot with the same item
        foreach (var slot in slots)
        {
            if (slot.Item.ReturnItemSO() != null && slot.Item.ReturnItemSO().ID == item.ReturnItemSO().ID && slot.Item.ReturnItemSO().StackMax > slot.Item.Stack)
            {
                return slot;
            }
        }

        //try to find empty slot
        foreach (var slot in slots)
        {
            if (slot.Item.ReturnItemSO() == null)
            {
                return slot;
            }
        }

        //coudln't find slot
        return null;
    }

}

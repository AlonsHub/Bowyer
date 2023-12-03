using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseInventory : MonoBehaviour
{
    public List<InventorySlot> slots;
    [HideInInspector] public UnityEvent OnInventoryChanged;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private int capacity;

    //should add type of mask of containable item types





    private void Awake()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < capacity; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, inventoryUI.slotGrid);
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
        while (itemsNum > 0 && slot != null)//keep adding items until stack reaches 0
        {
            itemsNum = slot.AddItem(item);
            slot = GetAvailableSlot(item);
        }
        OnInventoryChanged.Invoke();
        return itemsNum;
    }

    private InventorySlot GetAvailableSlot(ItemHolderData item)
    {
        //try to find a slot with the same item
        foreach (var slot in slots)
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

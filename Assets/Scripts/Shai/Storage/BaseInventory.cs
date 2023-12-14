using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseInventory : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnInventoryChanged;
    [SerializeField] protected List<InventorySlot> slots;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private int capacity;

    //should add type of mask of containable item types




    [ContextMenu("Trick")]
    public void Trick()
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

    //capcity check and fill/remove
    private void OnEnable()
    {
        CapacitySlotCheck();
    }

    private void CapacitySlotCheck()
    {
        if (slots != null && slots.Count != capacity)
        {
            int delta = slots.Count - capacity;

            if (delta < 0)
            {
                for (int i = 0; i < delta * -1; i++)
                {
                    GameObject slotGO = Instantiate(slotPrefab, inventoryUI.slotGrid);
                    InventorySlot newSlot = null;
                    if (slotGO.TryGetComponent<InventorySlot>(out newSlot))
                    {
                        slots.Add(newSlot);
                    }
                }
            }
            else
            {
                for (int i = 0; i < delta; i++)
                {
                    Destroy(slots[slots.Count - 1 - i].gameObject);
                    slots.RemoveAt(slots.Count - 1 - i);
                }
            }
        }
    }

    /// <summary>
    /// Returns item amount that cant be added to inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
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

    public virtual List<InventorySlot> GetAllSlots()
    {
        return slots;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform slotGrid;
    [SerializeField] private BaseInventory inventory;


    private void Awake()
    {
        inventory.OnInventoryChanged.AddListener(RefreshInventoryUI);
    }

    private void OnEnable()
    {
        RefreshInventoryUI();
    }

    [ContextMenu("Refresh")]
    public void RefreshInventoryUI()
    {
        foreach (var slot in inventory.GetAllSlots())
        {
            slot.UpdateUI();
        }
    }

    public void SetToInventory(BaseInventory newInventory)
    {
        inventory = newInventory;
        inventory.OnInventoryChanged.AddListener(RefreshInventoryUI);
    }
}

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
    }

    private void RefreshInventoryUI()
    {
        foreach (var slot in inventory.slots)
        {
            slot.UpdateUI();
        }
    }
}

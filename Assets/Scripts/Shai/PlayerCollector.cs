using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private Transform mainCam;
    [SerializeField] private BaseInventory playerInv;
    [SerializeField] private float collectRange;
    private ItemHolderData item;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryCollectItem();
        }
    }

    private void TryCollectItem()
    {
        if (Physics.Raycast(mainCam.position, mainCam.forward, out RaycastHit hit, collectRange))
        {
            if (hit.transform.TryGetComponent(out item))
            {
                int remainingItems = playerInv.AddItem(item);
                if (remainingItems < item.Stack)//checks if items were collected from the holder
                {
                    int subItems = item.Stack - remainingItems;//number of items that were collected from holder
                    Debug.Log(subItems.ToString() + " " + item.gameObject + " collected");
                    item.RemoveAmount(subItems);
                    if (item.Stack <= 0)//if collected holder stack reaches 0, destroy holder
                    {
                        Destroy(item.gameObject);
                    }
                }
            }
        }
    }
}


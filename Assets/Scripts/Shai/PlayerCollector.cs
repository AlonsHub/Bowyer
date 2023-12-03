using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private Transform mainCam;
    [SerializeField] private BaseInventory playerInv;
    [SerializeField] SphereCollider autoCollectColl;
    [SerializeField] private float collectRange;
    [SerializeField] bool isAutomatic;
    private ItemHolderData tmpItemHolder;

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
            if (hit.transform.TryGetComponent(out tmpItemHolder))
            {
                int remainingItems = playerInv.AddItem(tmpItemHolder);
                if (remainingItems < tmpItemHolder.Stack)//checks if items were collected from the holder
                {
                    int subItems = tmpItemHolder.Stack - remainingItems;//number of items that were collected from holder
                    Debug.Log(subItems.ToString() + " " + tmpItemHolder.gameObject + " collected");
                    tmpItemHolder.RemoveAmount(subItems);
                    if (tmpItemHolder.Stack <= 0)//if collected holder stack reaches 0, destroy holder
                    {
                        Destroy(tmpItemHolder.gameObject);
                    }
                }
                else
                {
                    Debug.Log("Can't collect " + tmpItemHolder.gameObject);
                }
            }
        }
    }

    public void IncreaseAutoCollectRange(float rangeToAdd )
    {
        float newR = autoCollectColl.radius + rangeToAdd;
        autoCollectColl.radius = newR < collectRange ? collectRange : newR;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAutomatic && other.transform.TryGetComponent(out tmpItemHolder))
        {
            int remainingItems = playerInv.AddItem(tmpItemHolder);
            if (remainingItems < tmpItemHolder.Stack)//checks if items were collected from the holder
            {
                int subItems = tmpItemHolder.Stack - remainingItems;//number of items that were collected from holder
                Debug.Log(subItems.ToString() + " " + tmpItemHolder.gameObject + " collected");
                tmpItemHolder.RemoveAmount(subItems);
                if (tmpItemHolder.Stack <= 0)//if collected holder stack reaches 0, destroy holder
                {
                    Destroy(tmpItemHolder.gameObject);
                }
            }
            else
            {
                Debug.Log("Can't collect " + tmpItemHolder.gameObject);
            }
        }
    }

    private void OnValidate()
    {
        if (autoCollectColl.radius != collectRange)
            autoCollectColl.radius = collectRange;
    }
}


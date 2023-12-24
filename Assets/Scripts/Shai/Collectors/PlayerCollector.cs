using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private BowsLogic bowsLogic;
    [SerializeField] private Transform mainCam;
    [SerializeField] private BaseInventory playerInv;
    [SerializeField] private CollectorCollider autoCollectColl;
    [SerializeField] private CollectorCollider collectorColl;
    [SerializeField] private float collectRange;
    [SerializeField] bool isAutomatic;
    private ItemHolderData tmpItemHolder;


    private void Awake()
    {
        autoCollectColl.OnTriggerEnterEvent.RemoveAllListeners();
        collectorColl.OnTriggerEnterEvent.RemoveAllListeners();

        autoCollectColl.coll.enabled = isAutomatic;
        autoCollectColl.OnTriggerEnterEvent.AddListener(Collect);
        collectorColl.OnTriggerEnterEvent.AddListener(Collect);
    }

    private void Update()
    {
        if (collectorColl.coll.enabled) { collectorColl.coll.enabled = false; }
        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            collectorColl.coll.enabled = true;
        }
    }

    private void TryCollectItem()
    {

        if (Physics.Raycast(mainCam.position, mainCam.forward, out RaycastHit hit, collectRange))
        {
            if (hit.transform.TryGetComponent(out tmpItemHolder) || hit.transform.parent.TryGetComponent(out tmpItemHolder))
            {
                Debug.Log("piss");
                if (tmpItemHolder.ReturnItemSO().Type == ItemType.Arrow)
                {
                    CollectArrow(tmpItemHolder);
                }
                else
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
    }

    public void IncreaseAutoCollectRange(float rangeToAdd)
    {
        float newR = ((SphereCollider)autoCollectColl.coll).radius + rangeToAdd;
        ((SphereCollider)autoCollectColl.coll).radius = newR < collectRange ? collectRange : newR;
    }



    private void Collect(Collider other)
    {
        if (other.transform.TryGetComponent(out tmpItemHolder))
        {
            if (tmpItemHolder.ReturnItemSO().Type == ItemType.Arrow)
            {
                CollectArrow(tmpItemHolder);
            }
            else
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
                    if (collectorColl.coll.enabled) { collectorColl.coll.enabled = false; }
                }
                else
                {
                    Debug.Log("Can't collect " + tmpItemHolder.gameObject);
                }
            }
        }
    }


    //parameter has to be an arrow item holder might want to add a deriving class from itemHolderData
    private void CollectArrow(ItemHolderData arrowItemHolder)
    {
        int remainingItems = bowsLogic.TryAddArrow(arrowItemHolder);
        if (remainingItems < arrowItemHolder.Stack)//checks if items were collected from the holder
        {
            int subItems = arrowItemHolder.Stack - remainingItems;//number of items that were collected from holder
            Debug.Log(subItems.ToString() + " " + arrowItemHolder.gameObject + " collected");
            arrowItemHolder.RemoveAmount(subItems);
            if (arrowItemHolder.Stack <= 0)//if collected holder stack reaches 0, destroy holder
            {
                Destroy(arrowItemHolder.gameObject);
            }
        }
        else
        {
            Debug.Log("Can't collect " + arrowItemHolder.gameObject);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (isAutomatic && other.transform.TryGetComponent(out tmpItemHolder))
    //    {
    //        if (tmpItemHolder.ReturnItemSO().Type == ItemType.Arrow)
    //        {
    //            CollectArrow(tmpItemHolder);
    //        }
    //        else
    //        {
    //            int remainingItems = playerInv.AddItem(tmpItemHolder);
    //            if (remainingItems < tmpItemHolder.Stack)//checks if items were collected from the holder
    //            {
    //                int subItems = tmpItemHolder.Stack - remainingItems;//number of items that were collected from holder
    //                Debug.Log(subItems.ToString() + " " + tmpItemHolder.gameObject + " collected");
    //                tmpItemHolder.RemoveAmount(subItems);
    //                if (tmpItemHolder.Stack <= 0)//if collected holder stack reaches 0, destroy holder
    //                {
    //                    Destroy(tmpItemHolder.gameObject);
    //                }
    //            }
    //            else
    //            {
    //                Debug.Log("Can't collect " + tmpItemHolder.gameObject);
    //            }
    //        }
    //    }
    //}

    private void OnValidate()
    {
        if (((SphereCollider)autoCollectColl.coll).radius != collectRange)
            ((SphereCollider)autoCollectColl.coll).radius = collectRange;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolderData : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;



    public ItemSO ReturnItemSO()
    {
        return itemSO;
    }
}

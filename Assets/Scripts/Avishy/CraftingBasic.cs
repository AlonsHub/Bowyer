using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBasic : MonoBehaviour
{
    /// THIS IS A GENERAL CRATING ZONE IN FRONT OF THE PLAYER!
    /// THIS CRAFTING ZONE IS ALWAYS ACTIVE AND ALWAYS ALLOWS THE PLAYER TO CRAFT AT ANY TIME!
    /// IN THE FUTURE, IF WE HAVE SPECIFIC CRAFTING STATIONS, THEY WILL NOT WORK IN THIS MANNER!
    /// THEY WILL INSTEAD HAVE THIER OWN COLLIDERS AND CRAFTING RECIPES ATTACHED TO THEM!
    /// BASIC CRAFTING RECIPE = RECIPE THAT CAN BE CRAFTED USING THIS SPECIFIC SYSTEM!



    [SerializeField] private BoxCollider craftingZoneCollider;
    [SerializeField] private Transform itemSpawnPoint;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            //try to craft
            Craft();
        }
    }

    private void Craft()
    {
        Collider[] collidersArray = Physics.OverlapBox(
            transform.position + craftingZoneCollider.center,
            craftingZoneCollider.size,
            craftingZoneCollider.transform.rotation);


        ///get all items currently in crafting zone.
        List<ItemSO> SOInCraftZone = new List<ItemSO>();
        List<GameObject> itemsInCraftZone = new List<GameObject>();     
        foreach (var collider in collidersArray)
        {
            if(collider.TryGetComponent(out ItemHolderData itemData))
            {
                SOInCraftZone.Add(itemData.ReturnItemSO());
                itemsInCraftZone.Add(collider.gameObject);
            }
        }

        /// find crafting recipe based on item list
        CraftingRecipeSO craftingSO = CraftingManager.instance.ReturnCraftingRecipeByItems(SOInCraftZone);

        if (craftingSO != null)
        {
            List<GameObject> itemsToConsume = new List<GameObject>();

            List<ItemSO> neededItems = new List<ItemSO>(craftingSO.neededItems);

            //Find all items to consume.
            foreach (var item in itemsInCraftZone)
            {
                if (item.TryGetComponent(out ItemHolderData itemData))
                {
                    if(neededItems.Contains(itemData.ReturnItemSO()))
                    {
                        neededItems.Remove(itemData.ReturnItemSO());
                        itemsToConsume.Add(item.gameObject);

                        if(neededItems.Count == 0)
                        {
                            break;
                        }
                    }
                }
            }

            //destroy items
            foreach (var consumedItem in itemsToConsume)
            {
                Destroy(consumedItem);
            }

            //spawn outcome
            Instantiate(craftingSO.outputItemSO.itemPrefab, itemSpawnPoint.position, itemSpawnPoint.transform.rotation);
            // if we're in here we already know we have the items to craft the item = success craft
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Create Crafting Recipe"/*, order = 1*/)]
public class CraftingRecipeSO : ScriptableObject
{
    public List<ItemSO> neededItems;
    public ItemSO outputItemSO;
}

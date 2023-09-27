using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager instance;
    [SerializeField] CraftingRecipeSO[] allBasicCraftingRecipes;


    private void Awake()
    {
        instance = this;
    }

    /// is it ok if we iterate through all recipies to find the correct one?
    /// this allows us to literally crafy anywhere at anytime, but is it optemized enoguh?
    /// what if we have hundreds of recipes?
    public CraftingRecipeSO ReturnCraftingRecipeByItems(List<ItemSO> itemsUsed)
    {
        foreach (var Recipe in allBasicCraftingRecipes)
        {
            List<ItemSO> neededItems = new List<ItemSO>();
            neededItems.AddRange(Recipe.neededItems);

            for (int i = 0; i < itemsUsed.Count; i++)
            {
                if (neededItems.Contains(itemsUsed[i]))
                {
                    neededItems.Remove(itemsUsed[i]);

                    if (neededItems.Count == 0)
                    {
                        return Recipe;
                    }
                }
            }
        }


        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class BowsLogic : MonoBehaviour
{
    [SerializeField] private List<BowQuiverset> sets;

    private int currentSetIndex;

    //remove current arrow from current set's quiver, update currentArrowIndex, instantiate arrow and shoot it
    //probably useless since shoot is called from an input in bow
    //public void Shoot()
    //{
    //    ArrowSO arrow = sets[currentSetIndex].Quiver.GetArrowAt(currentArrowIndex);
    //    BowShai bow = sets[currentSetIndex].Bow;
    //    sets[currentSetIndex].Quiver.RemoveArrowAt(currentArrowIndex);

    //    //now shoot or whatever idk
    //}

    //method name is very shit, change it
    public void ChangeCurrentArrowIndexByStep(int step)
    {
        sets[currentSetIndex].quiver.ChangeCurrentArrowIndexByStep(step);
        LoadBow();
    }

    public void ChangeCurrentArrowIndexByindex(int newIndex)
    {
        sets[currentSetIndex].quiver.ChangeCurrentArrowIndexByNum(newIndex);
        LoadBow();
    }

    public void ChangeCurrentSetIndexByStep(int step)
    {
        sets[currentSetIndex].bow.gameObject.SetActive(false);
        int newIndex = currentSetIndex += step;
        currentSetIndex = Mathf.Clamp(newIndex, 0, sets.Count - 1);
        sets[currentSetIndex].bow.gameObject.SetActive(true);

        //should update the bow the player sees
    }

    private void LoadBow()
    {
        //sets[currentSetIndex].bow.LoadArrow(sets[currentSetIndex].quiver.GetCurrentArrow().ItemPrefab);
        sets[currentSetIndex].Bow.LoadArrow(sets[currentSetIndex].quiver.GetCurrentArrow().ItemPrefab);
    }

    /// <summary>
    /// Get appropriate quiver and try to add the arrow to it.
    /// </summary>
    /// <param name="arrowHolderData"></param>
    /// <returns>remainder of stack</returns>
    public int TryAddArrow(ItemHolderData arrowHolderData)//should probably recieve a class deriving from IHD
    {
        int ogStack = arrowHolderData.Stack;
        Quiver tmpQuiver = null;
        foreach (var set in sets)
        {
            if ( set.quiver.SO.QuiverType == ((ArrowSO)arrowHolderData.ReturnItemSO()).ArrowType)
            {
                tmpQuiver = set.quiver;
            }
        }

        if (tmpQuiver)
        {

            if (((ArrowSO)arrowHolderData.ReturnItemSO()).IsSpecial)
            {
                return tmpQuiver.AddSpecialArrow(arrowHolderData);
            }
            else
            {
                return tmpQuiver.AddItem(arrowHolderData);
            }
        }
        else
        {
            Debug.Log("No appropriate quiver found");
            return ogStack;
        }
    }
}

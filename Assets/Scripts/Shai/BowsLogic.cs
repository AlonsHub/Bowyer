using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowsLogic : MonoBehaviour
{
    [SerializeField] private List<BowQuiverset> sets;
    private int currentSetIndex;
    private int currentArrowIndex;

    //remove current arrow from current set's quiver, update currentArrowIndex, instantiate arrow and shoot it
    //probably useless since shoot is called from an input in bow
    public void Shoot()
    {
        ArrowSO arrow = sets[currentSetIndex].Quiver.GetArrowAt(currentArrowIndex);
        BowShai bow = sets[currentSetIndex].Bow;
        sets[currentSetIndex].Quiver.RemoveArrowAt(currentArrowIndex);
        
        //now shoot or whatever idk
    }

    //method name is very shit, change it
    public void ChangeCurrentArrowIndexByStep(int step)
    {
        int newIndex = currentArrowIndex += step;
        currentArrowIndex = Mathf.Clamp(newIndex, 0, sets[currentSetIndex].quiver.slots.Count);
        //should update the arrow the player sees
    }

    public void ChangeCurrentArrowIndexByindex(int newIndex)
    {
        if (sets[currentSetIndex].quiver.slots[newIndex].Item.ReturnItemSO() != null)//if slot contains an arrow
        {
            currentArrowIndex = newIndex;
        }
        //should update the arrow the player sees
    }

    public void ChangeCurrentSetIndexByStep(int step)
    {
        int newIndex = currentSetIndex += step;
        currentSetIndex = Mathf.Clamp(newIndex, 0, sets.Count);


        //should update the bow the player sees
    }
}

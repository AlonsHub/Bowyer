using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //Alon changes begin

        //sets[currentSetIndex].bow.gameObject.SetActive(false);
        sets[currentSetIndex].bow.Holster();
        //int newIndex = currentSetIndex += step;
        //currentSetIndex = Mathf.Clamp(newIndex, 0, sets.Count - 1);
        //sets[currentSetIndex].bow.gameObject.SetActive(true);

        StartCoroutine(WaitForHolster(step));

        //Alon changes end


        //should update the bow the player sees
    }

    private IEnumerator WaitForHolster(int step)
    {
        yield return new WaitUntil(() => !sets[currentSetIndex].bow.gameObject.activeSelf);
        int newIndex = currentSetIndex += step;
        currentSetIndex = Mathf.Clamp(newIndex, 0, sets.Count - 1);
        sets[currentSetIndex].bow.gameObject.SetActive(true);

        PlayerController.CurrentBow = sets[currentSetIndex].bow;

        //_selectedItem.GetComponent<Animator>().SetTrigger("Equip");
    }

    private void LoadBow()
    {
        //sets[currentSetIndex].bow.LoadArrow(sets[currentSetIndex].quiver.GetCurrentArrow().ItemPrefab);
        sets[currentSetIndex].Bow.LoadArrow(sets[currentSetIndex].quiver.GetCurrentArrow().ItemPrefab);
    }
}

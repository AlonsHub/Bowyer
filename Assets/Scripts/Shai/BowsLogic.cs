using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class BowsLogic : MonoBehaviour
{
    [SerializeField] private List<BowQuiverset> sets;

    private int currentSetIndex;

    public System.Action OnEquipBow;
    public static System.Action<List<InventorySlot>> OnEquipQuiver;

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


    private void Start()
    {
        sets[0].bow.OnShoot.AddListener(() => sets[0].quiver.RemoveCurrentArrow());
        sets[1].bow.OnShoot.AddListener(() => sets[1].quiver.RemoveCurrentArrow());
        PlayerController.CurrentBow = sets[currentSetIndex].bow;

        OnEquipQuiver.Invoke(sets[currentSetIndex].quiver.GetAllSlots());
    }

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
        sets[currentSetIndex].bow.Holster();  

        StartCoroutine(WaitForHolster(step));

        //Alon changes end


        //should update the bow the player sees
    }

    private IEnumerator WaitForHolster(int step)
    {
        sets[currentSetIndex].bow.OnShoot.RemoveAllListeners();
        yield return new WaitUntil(() => !sets[currentSetIndex].bow.gameObject.activeSelf);
        
        int newIndex = currentSetIndex += step;
        if (newIndex >= sets.Count)
        {
            newIndex = 0;
        }
        if (newIndex < 0)
        {
            newIndex = sets.Count - 1;
        }
        //currentArrowIndex = Mathf.Clamp(newIndex, 0, slots.Count);
        currentSetIndex = newIndex;

        sets[currentSetIndex].bow.gameObject.SetActive(true);
        sets[currentSetIndex].bow.OnShoot.AddListener(()=> sets[currentSetIndex].quiver.RemoveCurrentArrow());

        PlayerController.CurrentBow = sets[currentSetIndex].bow;
        sets[currentSetIndex].quiver.HookToOwnUI();

        OnEquipQuiver.Invoke(sets[currentSetIndex].quiver.GetAllSlots());

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

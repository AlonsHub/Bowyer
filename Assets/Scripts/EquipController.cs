using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> equippedItems; //item 0 will be hands

    int _selectedIndex;
    GameObject _selectedItem;

    private void Awake()
    {
        SelectItem(0);
    }
    void Update() //temp! bad input grab, but good enough for now!
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            //if()
            SelectItem(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectItem(1);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectItem(3);
        }
    }

    public void SelectItem(int toSelect)
    {
        if(toSelect >= equippedItems.Count)
        {
            Debug.LogError("trying to select an item above current item count");
            return;
        }

        if(_selectedItem)
            _selectedItem.SetActive(false);
        
        if(_selectedIndex == toSelect)
        {
            _selectedIndex = -1;
            _selectedItem = null;
            return;
        }

        _selectedIndex = toSelect;
        _selectedItem = equippedItems[_selectedIndex];
        _selectedItem.SetActive(true);
    }
}

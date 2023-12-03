using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    [SerializeField] private PlayerInventoryManager itemManager;


    public PlayerInventoryManager ItemManager { get { return itemManager; } }





    private static BlackBoard instance;
    public static BlackBoard Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

}

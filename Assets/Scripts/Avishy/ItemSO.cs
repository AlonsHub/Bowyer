using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Create Item"/*, order = 1*/)]
public class ItemSO : ScriptableObject
{
    public GameObject itemPrefab;
}

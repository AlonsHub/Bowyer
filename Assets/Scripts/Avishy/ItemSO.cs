using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]public enum ItemType
{
    None = 0,
    Potion = 1 << 0,
    Arrow = 1 << 1,
    CraftingMaterial = 1 << 2,
    Mod = 1 << 3,
    Bow = 1 << 4,
    Equipment = 1 << 5,
    Quiver = 1 << 6
}

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Create Item"/*, order = 1*/)]
public class ItemSO : ScriptableObject
{
    [Header("Base item settings")]
    [SerializeField] private ItemType _type;
    [SerializeField] private string _name;
    [SerializeField] private string _id;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Sprite _icon;
    [SerializeField] private bool _isStackable;
    [SerializeField] private bool _isEquipable;
    [SerializeField] private bool _isCollectible;
    [SerializeField] private int _stackMax;


    public ItemType Type { get { return _type; } }
    public string ID { get { return _id; } }
    public string Name { get { return _name; } }
    public GameObject ItemPrefab { get { return _itemPrefab; } }
    public Sprite Icon { get { return _icon; } }
    public bool IsStackable { get { return _isStackable; } }
    public bool IsEquipable { get { return _isEquipable; } }
    public int StackMax { get { return _stackMax; } }
}

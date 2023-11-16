using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Potion,
    Arrow,
    CraftingMaterial,
    Mod,
    Bow,
    Equipment
}

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Create Item"/*, order = 1*/)]
public class ItemSO : ScriptableObject
{
    public ItemType Type { get { return _type; } }
    public string ID { get { return _id; } }
    public string Name { get { return _name; } }
    public GameObject ItemPrefab { get { return _itemPrefab; } }
    public Sprite Icon { get { return _icon; } }
    public bool IsStackable { get { return _isStackable; } }
    public int StackMax { get { return _stackMax; } }

    [SerializeField] private ItemType _type;
    [SerializeField] private string _name;
    [SerializeField] private string _id;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Sprite _icon;
    [SerializeField] private bool _isStackable;
    [SerializeField] private int _stackMax;
}

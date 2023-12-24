using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ArrowEffectType { None, Poison, Paralysis, Fire }

[CreateAssetMenu(fileName = "NewArrow", menuName = "ScriptableObjects/Items/Arrow")]
public class ArrowSO : ItemSO
{
    [Header("Arrow Settings")]
    [SerializeField] private ArrowEffectType effect;
    [SerializeField] private BowType arrowType;
    [SerializeField] private int damage;
    [SerializeField] private bool isSpecial;

    public ArrowEffectType Effect { get { return effect; } }
    public BowType ArrowType { get { return arrowType; } }
    public int Damage { get { return damage; } }
    public bool IsSpecial { get { return isSpecial; } }
}

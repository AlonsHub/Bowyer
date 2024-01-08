using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ArrowEffect { None, Poison, Paralysis, Fire }

[CreateAssetMenu(fileName = "NewArrow", menuName = "ScriptableObjects/Items/Arrow")]
public class ArrowSO : ItemSO
{
    [Header("Arrow Settings")]
    [SerializeField] private ArrowEffect effect;
    [SerializeField] private BowType arrowType;
    [SerializeField] private int damage;
    [SerializeField] private bool isSpecial;

    public ArrowEffect Effect { get { return effect; } }
    public BowType ArrowType { get { return arrowType; } }
    public int Damage { get { return damage; } }
    public bool IsSpecial { get { return isSpecial; } }
}

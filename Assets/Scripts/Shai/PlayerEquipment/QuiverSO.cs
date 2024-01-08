using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuiver", menuName = "ScriptableObjects/Items/Quiver")]
public class QuiverSO : ItemSO
{
    [Header("Quiver Settings")]
    [SerializeField] private BowType quiverType;
    [SerializeField] private int startCapacity;
    [SerializeField] private int maxCapacity;

    public BowType QuiverType { get { return quiverType; } }
    public int StartCapacity { get { return startCapacity; } }
    public int MaxCapacity { get { return maxCapacity; } }
}


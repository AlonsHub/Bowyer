using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuiverType { Short, Recurve, Long}

[CreateAssetMenu(fileName = "NewQuiver", menuName = "ScriptableObjects/Items/Quiver")]
public class QuiverSO : ItemSO
{
    [Header("Quiver Settings")]
    [SerializeField] private QuiverType quiverType;
    [SerializeField] private int startCapacity;
    [SerializeField] private int maxCapacity;

    public QuiverType QuiverType { get { return quiverType; } }
    public int StartCapacity { get { return startCapacity; } }
    public int MaxCapacity { get { return maxCapacity; } }
}


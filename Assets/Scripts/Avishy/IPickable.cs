using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    abstract void PickUp(Transform pickUpPoint);
    abstract void Drop();
}

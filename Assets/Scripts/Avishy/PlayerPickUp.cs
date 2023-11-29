using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform mainCam;
    [SerializeField] private float pickUpDistance;
    [SerializeField] private LayerMask pickUpLayer;
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private IPickable pickableItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(pickableItem == null)
            {
                if (Physics.Raycast(mainCam.position, mainCam.forward, out RaycastHit hit, pickUpDistance, pickUpLayer))
                {
                    if (hit.transform.TryGetComponent(out pickableItem))
                    {
                        Debug.Log(pickableItem);
                        pickableItem.PickUp(pickUpPoint);
                    }
                }
            }
            else
            {
                pickableItem.Drop();
                pickableItem = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if(pickableItem == null)
            {
                if (Physics.Raycast(mainCam.position, mainCam.forward, out RaycastHit hit, pickUpDistance, pickUpLayer))
                {
                    if (hit.transform.TryGetComponent(out pickableItem))
                    {
                        pickableItem.PlaySound();
                        Destroy(pickableItem.ReturnConnectedGO());
                    }
                }
            }
        }
    }
}

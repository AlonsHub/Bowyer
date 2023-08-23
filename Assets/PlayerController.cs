using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //handles input grab and processing
    //controls the player character, NOT THE BOW!
    //the player interacts with the bow as an interface - and that can recat however it would like
    //
    [SerializeField]
    CharacterController cc;
    [SerializeField]
    float walkSpeed;

    Vector3 _inputVector;
    // Start is called before the first frame update
    void Awake()
    {
        if (!cc)
        {
            cc = GetComponent<CharacterController>(); //this actually must exist, since it is a required component
        }    
    }

    // Update is called once per frame
    void Update()
    {
        _inputVector = Vector3.zero;

        _inputVector = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
        
        if (_inputVector.magnitude > 1)
            _inputVector.Normalize();

        //temp!
        _inputVector *= walkSpeed;

        if (!cc.isGrounded)
            _inputVector += Physics.gravity;

        _inputVector *= Time.deltaTime;
        Move();
    }

    void Move()
    {
        cc.Move(_inputVector);
    }
}

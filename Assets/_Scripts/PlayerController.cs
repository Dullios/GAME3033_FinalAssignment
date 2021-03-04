using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Components
    private Animator anim;
    private Rigidbody rigidBody;

    private Vector2 inputVector = Vector2.zero;

    //Animator Hashes
    public readonly int MovementXHash = Animator.StringToHash("MovementX");
    public readonly int MovementYHash = Animator.StringToHash("MovementY");
    public readonly int IsRunningHash = Animator.StringToHash("isRunning");

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        anim.SetFloat(MovementXHash, inputVector.x);
        anim.SetFloat(MovementYHash, inputVector.y);
    }

    public void OnRun(InputValue value)
    {
        anim.SetBool(IsRunningHash, value.isPressed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum PlayerState
{
    WALK,
    RUN,
    JUMP
}

public class PlayerController : MonoBehaviour
{
    // Player States
    [SerializeField]
    private PlayerState playerState;

    // Components
    private Animator anim;
    private Rigidbody rigidBody;

    [Header("Movement")]
    [SerializeField] private Vector2 inputVector = Vector2.zero;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    //Animator Hashes
    public readonly int MovementXHash = Animator.StringToHash("MovementX");
    public readonly int MovementYHash = Animator.StringToHash("MovementY");
    public readonly int IsRunningHash = Animator.StringToHash("isRunning");
    public readonly int IsJumpingHash = Animator.StringToHash("isJumping");

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 movementDirection = Vector3.zero;

        switch (playerState)
        {
            case PlayerState.WALK:
                moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;

                movementDirection = moveDirection * (walkSpeed * Time.deltaTime);

                transform.position += movementDirection;
                break;
            
            case PlayerState.RUN:
                moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;

                movementDirection = moveDirection * (runSpeed * Time.deltaTime);

                transform.position += movementDirection;
                break;
            
            case PlayerState.JUMP:
                
                break;
        }
    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        anim.SetFloat(MovementXHash, inputVector.x);
        anim.SetFloat(MovementYHash, inputVector.y);
    }

    public void OnRun(InputValue value)
    {
        if (playerState != PlayerState.JUMP)
        {
            anim.SetBool(IsRunningHash, value.isPressed);

            playerState = value.isPressed ? PlayerState.RUN : PlayerState.WALK;
        }
    }

    public void OnJump(InputValue value)
    {
        if(playerState != PlayerState.JUMP)
        {
            anim.SetBool(IsJumpingHash, value.isPressed);

            float force = playerState == PlayerState.RUN ? runSpeed : walkSpeed;
            Vector3 forceVector = (transform.up * jumpForce) + (moveDirection * force);

            rigidBody.AddForce(forceVector, ForceMode.Impulse);
            
            playerState = PlayerState.JUMP;
        }
    }

    public void OnCrouch(InputValue value)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(playerState == PlayerState.JUMP)
        {
            anim.SetBool(IsJumpingHash, false);
            playerState = PlayerState.WALK;
        }
    }
}

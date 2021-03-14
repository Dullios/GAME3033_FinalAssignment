using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum PlayerState
{
    WALK,
    RUN,
    JUMP,
    ATTACK
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

    [Header("Camera")]
    [SerializeField] private float rotationPower;
    [SerializeField] private float horizontalDampening = 1.0f;
    private Vector2 previousMouseDelta = Vector2.zero;

    private int comboStep;
    private bool comboPossible;

    //Animator Hashes
    public readonly int MovementXHash = Animator.StringToHash("MovementX");
    public readonly int MovementYHash = Animator.StringToHash("MovementY");
    public readonly int IsRunningHash = Animator.StringToHash("isRunning");
    public readonly int IsJumpingHash = Animator.StringToHash("isJumping");
    
    public readonly int Combo1Hash = Animator.StringToHash("Combo1");
    public readonly int Combo2Hash = Animator.StringToHash("Combo2");
    public readonly int Combo3Hash = Animator.StringToHash("Combo3");

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

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

    private void OnLook(InputValue value)
    {
        if (playerState != PlayerState.ATTACK)
        {
            Vector2 lookValue = value.Get<Vector2>();

            Quaternion addedRotation = Quaternion.AngleAxis(Mathf.Lerp(previousMouseDelta.x, lookValue.x, 1.0f / horizontalDampening) * rotationPower, transform.up);

            transform.rotation *= addedRotation;

            previousMouseDelta = lookValue;
        }
    }

    private void OnAttack(InputValue value)
    {
        if (playerState == PlayerState.WALK)
        {
            anim.Play("Combo 1");
            comboStep++;
            playerState = PlayerState.ATTACK;
        }
        else if (comboStep != 0)
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboStep++;
            }
        }
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {
        if (comboStep == 2)
            anim.Play("Combo 2");
        if (comboStep == 3)
        {
            anim.Play("Combo 3");

            rigidBody.AddForce(-Vector3.forward * runSpeed, ForceMode.Impulse);
        }
    }

    public void ComboReset()
    {
        comboPossible = false;
        comboStep = 0;
        playerState = PlayerState.WALK;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Components
    private Animator anim;
    private Rigidbody rigidBody;
    private PlayerStats playerStats;

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
    public readonly int IsBlockingHash = Animator.StringToHash("isBlocking");

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (!playerStats.isJumping && !playerStats.isAttacking && !playerStats.isBlocking)
        {
            moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;

            float speed = playerStats.isRunning ? runSpeed : walkSpeed;
            Vector3 movementDirection = moveDirection * (speed * Time.deltaTime);

            transform.position += movementDirection;
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
        if (!playerStats.isJumping)
        {
            anim.SetBool(IsRunningHash, value.isPressed);

            playerStats.isRunning = value.isPressed;
        }
    }

    public void OnJump(InputValue value)
    {
        if(!playerStats.isJumping)
        {
            anim.SetBool(IsJumpingHash, value.isPressed);

            float force = playerStats.isRunning ? runSpeed : walkSpeed;
            Vector3 forceVector = (transform.up * jumpForce) + (moveDirection * force);

            rigidBody.AddForce(forceVector, ForceMode.Impulse);
            
            playerStats.isJumping = true;
        }
    }

    public void OnCrouch(InputValue value)
    {

    }

    public void OnLook(InputValue value)
    {
        if (!playerStats.isAttacking && !playerStats.isBlocking)
        {
            Vector2 lookValue = value.Get<Vector2>();

            Quaternion addedRotation = Quaternion.AngleAxis(Mathf.Lerp(previousMouseDelta.x, lookValue.x, 1.0f / horizontalDampening) * rotationPower, transform.up);

            transform.rotation *= addedRotation;

            previousMouseDelta = lookValue;
        }
    }

    public void OnAttack(InputValue value)
    {
        if (!playerStats.isAttacking && !playerStats.isBlocking)
        {
            anim.Play("Combo 1");
            comboStep++;
            playerStats.isAttacking = true;
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
        playerStats.isAttacking = false;
    }

    public void OnBlock(InputValue value)
    {
        anim.SetBool(IsBlockingHash, value.isPressed);

        playerStats.isBlocking = value.isPressed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(playerStats.isJumping)
        {
            anim.SetBool(IsJumpingHash, false);
            playerStats.isJumping = false;
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            if(!playerStats.hasBlocked)
            {
                Debug.Log("Took Damage!");
            }
            else
            {
                Debug.Log("Blocked Damage!");
                playerStats.hasBlocked = false;
            }
        }
    }
}

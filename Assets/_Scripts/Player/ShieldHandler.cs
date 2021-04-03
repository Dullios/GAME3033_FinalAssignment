using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldHandler : MonoBehaviour
{
    private BoxCollider boxCollider;
    public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    public void OnBlock(InputValue value)
    {
        boxCollider.enabled = value.isPressed;
        if (!value.isPressed)
            playerStats.hasBlocked = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Shield Blocked!");
            playerStats.hasBlocked = true;
        }
    }
}

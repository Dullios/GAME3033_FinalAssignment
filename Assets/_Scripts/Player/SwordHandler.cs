using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordHandler : MonoBehaviour
{
    public BoxCollider swordCollider;
    private PlayerStats playerStats;
    public GameObject damageBillboard;

    // Start is called before the first frame update
    void Start()
    {
        swordCollider = GetComponent<BoxCollider>();
        swordCollider.enabled = false;

        playerStats = GetComponentInParent<PlayerStats>();
    }

    public void OnAttack(InputValue value)
    {
        swordCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float comboDmg = (GetComponentInParent<PlayerController>().comboStep * 0.3f) * playerStats.damage;
            comboDmg += playerStats.damage;

            Vector3 pos = collision.contacts[0].point;
            GameObject dmgBillboard = Instantiate(damageBillboard, pos, Quaternion.identity);
            dmgBillboard.GetComponentInChildren<TMP_Text>().text = comboDmg.ToString();

            collision.gameObject.GetComponentInParent<EnemyStats>().DealDamage((int)comboDmg);
            Debug.Log("Hit Enemy!");
        }
    }
}

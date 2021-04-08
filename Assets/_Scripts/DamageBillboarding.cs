using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBillboarding : MonoBehaviour
{
    public float speed;
    private float endYPos;
    private float timer;

    private void Start()
    {
        endYPos = transform.position.y;
        endYPos += 2;
    }

    // Update is called once per frame
    void Update()
    {
        timer += speed * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(transform.position.y, endYPos, timer);
        transform.position = pos;

        transform.LookAt(transform.position + Camera.main.transform.forward);

        if (timer >= 1)
            Destroy(gameObject);
    }
}

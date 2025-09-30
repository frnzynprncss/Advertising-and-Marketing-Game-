using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAutomatic : MonoBehaviour
{
    private Rigidbody2D rb;
    private float[] xDirection = new float[3]{-8f, 0f ,8f};
    private int xDirectionRandomIndex;
    private float yDirection = -4.5f;
    private float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        xDirectionRandomIndex = Random.Range(0, xDirection.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 direction = new Vector2(xDirection[xDirectionRandomIndex], yDirection).normalized;
        Vector2 movement = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}

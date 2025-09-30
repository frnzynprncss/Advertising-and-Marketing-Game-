using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInput : MonoBehaviour
{
    private float speed = 6.0f;
    private Rigidbody2D rb;
    private float deltaX;
    private enum LanePos
    {
        Mid = 0,
        Right = 1,
        Left = 2
    }
    [SerializeField] private LanePos lanePos = LanePos.Left;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        checkPos();
        if (Input.GetMouseButtonDown(0))
        {
            float screenMidX = Screen.width / 2f;
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x < screenMidX)
            {
                deltaX = -1f;
            }
            else
            {
                deltaX = 1f;
            }
        }
    }

    private void checkPos()
    {
        if (transform.position.x >= 0 && lanePos == LanePos.Left)
        {
            deltaX = 0f;
            lanePos = LanePos.Mid;
        }
        else if (transform.position.x <= 0 && lanePos == LanePos.Right)
        {
            deltaX = 0f;
            lanePos = LanePos.Mid;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LeftWall"))
        {
            lanePos = LanePos.Left;
        }
        if (collision.gameObject.CompareTag("RightWall"))
        {
            lanePos = LanePos.Right;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(deltaX * speed, 0);
    }


}

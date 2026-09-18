using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float JumpHeight;
    public float MovementSpeed;

    Rigidbody2D RB;

    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float XAxis = Input.GetAxis("Horizontal");
        RB.velocity = new Vector2(XAxis * MovementSpeed, RB.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            RB.velocity += new Vector2(0, JumpHeight);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadScript : MonoBehaviour
{
    public float BounceHeight;
    AudioSource AS;

    void Start()
    {
        AS = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D RB = collision.gameObject.GetComponent<Rigidbody2D>();
            RB.velocity = new Vector2(RB.velocity.x, BounceHeight);
            AS.Play();
        }
    }
}

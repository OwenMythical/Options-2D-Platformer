using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    GameObject Player;
    Rigidbody2D RB;
    GameObject GameController;
    AudioSource AS;
    AudioLowPassFilter ALPF;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        RB = Player.GetComponent<Rigidbody2D>();
        GameController = GameObject.FindGameObjectWithTag("GameController");
        AS = GameController.GetComponent<AudioSource>();
        ALPF = GameController.GetComponent<AudioLowPassFilter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ALPF.cutoffFrequency = 2000f;
            RB.gravityScale = 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ALPF.cutoffFrequency = 22000f;
            RB.gravityScale = 1f;
        }
    }
}

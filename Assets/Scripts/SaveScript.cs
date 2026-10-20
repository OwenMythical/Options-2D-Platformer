using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveScript : MonoBehaviour
{
    public int ID;
    public Canvas SaveDisplay;
    GameManager GM;
    Collider2D Collider;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        Collider = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Collider2D PlayerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
            if (Collider.IsTouching(PlayerCollider))
            {
                GM.SaveGame(SceneManager.GetActiveScene().name, ID);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SaveDisplay.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SaveDisplay.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveScript : MonoBehaviour
{
    public int ID;
    GameManager GM;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GM.SaveGame(SceneManager.GetActiveScene().name, ID);
        }
    }
}

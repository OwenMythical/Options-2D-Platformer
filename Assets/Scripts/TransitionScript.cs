using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    public string TargetScene;
    public int TargetSpawnID;
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
            GM.SceneTransition(TargetScene, TargetSpawnID);
        }
    }
}

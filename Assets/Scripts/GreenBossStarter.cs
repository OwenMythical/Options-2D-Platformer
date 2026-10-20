using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBossStarter : MonoBehaviour
{
    public GreenBossMovement GBM;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GBM.StartBoss();
            GameObject.Destroy(gameObject);
        }
    }
}

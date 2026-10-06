using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    GameObject Player;
    public float XSpeed;
    public float YSpeed;

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        if (XSpeed == 0)
        {
            XSpeed = Mathf.Infinity;
        }
        if (YSpeed == 0)
        {
            YSpeed = Mathf.Infinity;
        }
        gameObject.transform.position = new Vector3(-Player.transform.position.x/XSpeed, -Player.transform.position.y/YSpeed, 0);
    }
}

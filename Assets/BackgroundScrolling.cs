using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    public GameObject Player;
    public float XSpeed;
    public float YSpeed;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(-Player.transform.position.x/XSpeed, -Player.transform.position.y/YSpeed, 0);
    }
}

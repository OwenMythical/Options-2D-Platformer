using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(-Player.transform.position.x/10, 0, 0);
    }
}

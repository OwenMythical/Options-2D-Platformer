using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RedSlimeMovement : MonoBehaviour
{

    public GameObject TileChecker;
    Tilemap TM;

    // Start is called before the first frame update
    void Start()
    {
        TM = GameObject.FindGameObjectWithTag("CollideMap").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int Position = Vector3Int.FloorToInt(TileChecker.transform.position);
        TileBase T = TM.GetTile(Position);
        if (T == null)
        {
            gameObject.transform.rotation = new Quaternion(0, gameObject.transform.rotation.y+180, 0, 0);
            Debug.Log(gameObject.transform.rotation.y);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GreenSlimeMovement : MonoBehaviour
{
    public float XSpeed;
    float CurrentSpeed;
    public float JumpHeight;
    public bool Dead;
    public GameObject TileChecker;
    public GameObject WallTileChecker;
    public GameObject WallTileChecker2;
    public ParticleSystem PS;
    Tilemap TM;
    Rigidbody2D RB;
    AudioSource AS;
    Animator AN;
    bool Paused = false;

    // Start is called before the first frame update
    void Start()
    {
        TM = GameObject.FindGameObjectWithTag("CollideMap").GetComponent<Tilemap>();
        RB = gameObject.GetComponent<Rigidbody2D>();
        AS = gameObject.GetComponent<AudioSource>();
        AN = gameObject.GetComponent<Animator>();
        PS.Play();
        CurrentSpeed = XSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Paused == false && Dead == false)
        {
            if (gameObject.transform.rotation.y == 0)
            {
                RB.velocity = new Vector2(CurrentSpeed, RB.velocity.y);
            }
            else
            {
                RB.velocity = new Vector2(-CurrentSpeed, RB.velocity.y);
            }

            Vector3Int Position = Vector3Int.FloorToInt(TileChecker.transform.position);
            TileBase T = TM.GetTile(Position);
            Vector3Int Position2 = Vector3Int.FloorToInt(WallTileChecker.transform.position);
            TileBase T2 = TM.GetTile(Position2);
            Vector3Int Position3 = Vector3Int.FloorToInt(WallTileChecker2.transform.position);
            TileBase T3 = TM.GetTile(Position3);
            if (T2 != null)
            {
                if (T3 == null && Mathf.Approximately(RB.velocity.y, 0f))
                {
                    RB.velocity = new Vector2(RB.velocity.x, JumpHeight);
                    CurrentSpeed = 2;
                    StartCoroutine(ReturnSpeed());
                }
                else if (Mathf.Approximately(RB.velocity.y, 0f))
                {
                    StartCoroutine(WaitBeforeTurning());
                }
            }

            if (T == null)
            {
                StartCoroutine(WaitBeforeTurning());
            }
        }
        AN.SetBool("Walking", !Paused);
    }

    IEnumerator WaitBeforeTurning()
    {
        Paused = true;
        RB.velocity = new Vector2(0, 0);
        PS.Stop();
        yield return new WaitForSeconds(2);
        if (gameObject.transform.rotation.y == 0)
        {
            gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        if (Dead == false)
        {
            PS.Play();
        }
        Paused = false;
    }

    IEnumerator ReturnSpeed()
    {
        yield return new WaitForSeconds(1);
        CurrentSpeed = XSpeed;
    }
}
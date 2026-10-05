using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RedSlimeMovement : MonoBehaviour
{
    public float XSpeed;
    public bool Dead;
    public GameObject TileChecker;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Paused == false && Dead == false)
        {
            if (gameObject.transform.rotation.y == 0)
            {
                RB.velocity = new Vector2(XSpeed, 0);
            }
            else
            {
                RB.velocity = new Vector2(-XSpeed, 0);
            }

            if (RB.velocity.x == 0)
            {
                if (gameObject.transform.rotation.y == 0)
                {
                    gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }

            Vector3Int Position = Vector3Int.FloorToInt(TileChecker.transform.position);
            TileBase T = TM.GetTile(Position);
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
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GreenBossMovement : MonoBehaviour
{
    public float XSpeed;
    public float CurrentSpeed;
    public float JumpHeight;
    public bool Dead;
    public GameObject TileChecker;
    public GameObject WallTileChecker;
    public GreenBossHurtbox GBH;
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
        CurrentSpeed = XSpeed;
    }

    public void StartBoss()
    {
        StartCoroutine(AttackPattern());
    }

    IEnumerator AttackPattern()
    {
        GBH.Recovering = false;
        yield return new WaitForSeconds(1);
        int Rnd = Random.Range(1, 3);
        if (Rnd == 1)
        {
            CurrentSpeed = 2.5f;
            StartCoroutine(Attack1());
        }
        if (Rnd == 2)
        {
            CurrentSpeed = 4f;
            StartCoroutine(Attack2());
        }
    }

    IEnumerator Attack1()
    {
        AN.SetBool("Walking", true);
        if (gameObject.transform.rotation.y == 0)
        {
            while (gameObject.transform.position.x < -182.5f)
            {
                RB.velocity = new Vector2(CurrentSpeed, RB.velocity.y);

                Vector3Int Position = Vector3Int.FloorToInt(TileChecker.transform.position);
                TileBase T = TM.GetTile(Position);

                if (T != null)
                {
                    RB.velocity = new Vector2(RB.velocity.x, JumpHeight);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            while (gameObject.transform.position.x > -196.5f)
            {
                RB.velocity = new Vector2(-CurrentSpeed, RB.velocity.y);

                Vector3Int Position = Vector3Int.FloorToInt(TileChecker.transform.position);
                TileBase T = TM.GetTile(Position);

                if (T != null)
                {
                    RB.velocity = new Vector2(RB.velocity.x, JumpHeight);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        RB.velocity = new Vector2(0, RB.velocity.y);
        AN.SetBool("Walking", false);
        yield return new WaitForSeconds(2f);
        if (gameObject.transform.rotation.y == 0)
        {
            gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        StartCoroutine(AttackPattern());
    }

    IEnumerator Attack2()
    {
        AN.SetBool("Walking", true);
        if (gameObject.transform.rotation.y == 0)
        {
            bool Jumped = false;
            while (gameObject.transform.position.x < -182.5f)
            {
                RB.velocity = new Vector2(CurrentSpeed, RB.velocity.y);

                Vector3Int Position = Vector3Int.FloorToInt(WallTileChecker.transform.position);
                TileBase T = TM.GetTile(Position);

                if (T != null && Jumped == false)
                {
                    Jumped = true;
                    CurrentSpeed = 2f;
                    RB.velocity = new Vector2(RB.velocity.x, JumpHeight);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            bool Jumped = false;
            while (gameObject.transform.position.x > -196.5f)
            {
                RB.velocity = new Vector2(-CurrentSpeed, RB.velocity.y);

                Vector3Int Position = Vector3Int.FloorToInt(WallTileChecker.transform.position);
                TileBase T = TM.GetTile(Position);

                if (T != null && Jumped == false)
                {
                    Jumped = true;
                    CurrentSpeed = 2f;
                    RB.velocity = new Vector2(RB.velocity.x, JumpHeight);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        RB.velocity = new Vector2(0, RB.velocity.y);
        AN.SetBool("Walking", false);
        yield return new WaitForSeconds(2f);
        if (gameObject.transform.rotation.y == 0)
        {
            gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        StartCoroutine(AttackPattern());
    }
}
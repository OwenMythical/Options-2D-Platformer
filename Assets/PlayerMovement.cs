using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float JumpHeight;
    public float SuperJumpHeight;
    public float MovementSpeed;
    public ParticleSystem PS;
    public GameObject Cam;

    float SuperJumpTimer = 0;
    bool SuperJumping = false;
    bool SuperLeaping = false;
    float SuperJumpMultiplier = 1;
    float SuperChargeMultiplier = 1;

    Rigidbody2D RB;
    Animator AN;
    SpriteRenderer SR;

    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();
        AN = gameObject.GetComponent<Animator>();
        SR = gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine(SuperJumpFlicker());
    }

    void Update()
    {
        float XAxis = Input.GetAxis("Horizontal");
        float XAxisRaw = Input.GetAxisRaw("Horizontal");


        //Walking + Sliding
        if (SuperLeaping == false)
        {
            if (XAxis > 0 && XAxisRaw == 1)
            {
                AN.SetBool("Walking", true);
                SR.flipX = false;
                if (PS.isPlaying == false && Mathf.Approximately(RB.velocity.y, 0f) == true)
                {
                    PS.Play();
                }
                PS.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (XAxis > 0 && XAxisRaw == 0)
            {
                AN.SetBool("Sliding", true);
                AN.SetBool("Walking", false);
                SR.flipX = false;
                PS.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (XAxis < 0 && XAxisRaw == -1)
            {
                AN.SetBool("Walking", true);
                SR.flipX = true;
                if (PS.isPlaying == false && Mathf.Approximately(RB.velocity.y, 0f) == true)
                {
                    PS.Play();
                }
                PS.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else if (XAxis < 0 && XAxisRaw == 0)
            {
                AN.SetBool("Sliding", true);
                AN.SetBool("Walking", false);
                SR.flipX = true;
                PS.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                AN.SetBool("Walking", false);
                AN.SetBool("Sliding", false);
                PS.Stop();
            }
            RB.velocity = new Vector2(XAxis * MovementSpeed * SuperJumpMultiplier * SuperChargeMultiplier, RB.velocity.y);
        }

        //Jumping
        if (Input.GetKey(KeyCode.Space) == true && Mathf.Approximately(RB.velocity.y, 0f) == true)
        {
            if (SuperJumpTimer > 2.5)
            {
                RB.velocity += new Vector2(0, SuperJumpHeight);
                SuperJumpTimer = 0;
                SuperJumping = true;
                AN.SetBool("SuperJumping", true);

                if (Mathf.Abs(XAxis) == 1 && SuperChargeMultiplier == 1)
                {
                    SuperLeaping = true;
                }
                else
                {
                    SuperJumpMultiplier = 0.25f;
                }
            }
            else
            {
                RB.velocity += new Vector2(0, JumpHeight);
            }
            PS.Stop();
        }
        if (RB.velocity.y < 0 && SuperJumping == true)
        {
            SuperJumping = false;
            AN.SetBool("SuperJumping", false);
            if (SuperLeaping == false)
            {
                StartCoroutine(RestoreSuperJumpSpeed());
            }
            SuperLeaping = false;
        }

        //Up and Down
        if (Input.GetKey(KeyCode.W) == true && Mathf.Approximately(RB.velocity.y, 0f) == true)
        {
            Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y + 1.5f, 0.05f), -10);
            SuperChargeMultiplier = 0.25f;

            if (SuperJumpTimer < 3)
            {
                SuperJumpTimer += Time.deltaTime;
                if (SuperJumpTimer > 1)
                {
                    SuperJumpTimer = 3;
                }
            }
        }
        else if (SuperJumpTimer > 0)
        {
            SuperJumpTimer -= Time.deltaTime;
            if (SuperJumpTimer < 0)
            {
                SuperJumpTimer = 0;
            }
        }
        if (Input.GetKey(KeyCode.S) == true && Mathf.Approximately(RB.velocity.y, 0f) == true)
        {
            Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y - 1.5f, 0.05f), -10);
            SuperChargeMultiplier = 0.25f;
        }
        if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false || Mathf.Approximately(RB.velocity.y, 0f) == false)
        {
            Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y, 0.05f), -10);
            SuperChargeMultiplier = 1f;
        }

        //Misc
        AN.SetFloat("YVelocity", Mathf.RoundToInt(RB.velocity.y));
        if (Mathf.Approximately(RB.velocity.y, 0f) == false)
        {
            PS.Stop();
        }
    }

    IEnumerator SuperJumpFlicker()
    {
        bool Toggle = false;
        while (true)
        {
            if (SuperJumpTimer > 2.5)
            {
                if (Toggle == false)
                {
                    Toggle = true;
                    SR.color = new Color32(255, 225, 255, 255);
                }
                else
                {
                    Toggle = false;
                    SR.color = new Color32(255, 255, 255, 255);
                }
            }
            else if (Toggle == true)
            {
                Toggle = false;
                SR.color = new Color32(255, 255, 255, 255);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator RestoreSuperJumpSpeed()
    {
        while (SuperJumpMultiplier < 1)
        {
            SuperJumpMultiplier += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}

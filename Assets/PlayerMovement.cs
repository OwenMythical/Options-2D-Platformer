using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float JumpHeight;
    public float MovementSpeed;
    public ParticleSystem PS;
    public GameObject Cam;

    Rigidbody2D RB;
    Animator AN;
    SpriteRenderer SR;

    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();
        AN = gameObject.GetComponent<Animator>();
        SR = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float XAxis = Input.GetAxis("Horizontal");
        float XAxisRaw = Input.GetAxisRaw("Horizontal");
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

        RB.velocity = new Vector2(XAxis * MovementSpeed, RB.velocity.y);

        if (Input.GetKey(KeyCode.Space) == true && Mathf.Approximately(RB.velocity.y, 0f) == true)
        {
            RB.velocity += new Vector2(0, JumpHeight);
            PS.Stop();
        }
        if (Input.GetKey(KeyCode.W) == true && Mathf.Approximately(RB.velocity.y, 0f) == true && Mathf.Approximately(RB.velocity.x, 0f) == true)
        {
            Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y + 1.5f, 0.05f), -10);
        }
        if (Input.GetKey(KeyCode.S) == true && Mathf.Approximately(RB.velocity.y, 0f) == true && Mathf.Approximately(RB.velocity.x, 0f) == true)
        {
            Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y - 1.5f, 0.05f),-10);
        }
        if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false || Mathf.Approximately(RB.velocity.y, 0f) == false || Mathf.Approximately(RB.velocity.x, 0f) == false)
        {
            Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y, 0.05f), -10);
        }

            AN.SetFloat("YVelocity", Mathf.RoundToInt(RB.velocity.y));

        if (Mathf.Approximately(RB.velocity.y, 0f) == false)
        {
            PS.Stop();
        }
    }
}

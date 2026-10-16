using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public int Health;
    public int MaxHealth;
    public float CameraOffset;
    public float JumpHeight;
    public float SuperJumpHeight;
    public float MovementSpeed;
    public ParticleSystem PS;
    public GameObject Cam;
    public BoxCollider2D GroundChecker;
    public PhysicsMaterial2D PhysicsObj;

    float SuperJumpTimer = 0;
    bool SuperJumping = false;
    bool SuperLeaping = false;
    float SuperJumpMultiplier = 1;
    float SuperChargeMultiplier = 1;

    public bool Stunned = false;
    float StunMultiplier = 1;
    public float KnockbackForce;
    public bool TransitionStun = false;

    Rigidbody2D RB;
    Animator AN;
    SpriteRenderer SR;
    BoxCollider2D BC;
    GameManager GM;
    CompositeCollider2D CollideMap;
    CompositeCollider2D OneWayMap;
    Camera CamComponent;

    AudioSource AS;
    public AudioClip ChargedSound;
    public AudioClip JumpSound;
    public AudioClip SuperJumpSound;
    public AudioClip HurtSound;
    public GameObject AudioPlayer;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            RB = gameObject.GetComponent<Rigidbody2D>();
            AN = gameObject.GetComponent<Animator>();
            SR = gameObject.GetComponent<SpriteRenderer>();
            BC = gameObject.GetComponent<BoxCollider2D>();
            AS = gameObject.GetComponent<AudioSource>();
            GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            CamComponent = Cam.GetComponent<Camera>();

            StartCoroutine(SuperJumpFlicker());
        }
    }

    float XAxis = 0f;
    float Smoothing = 0.05f;
    void Update()
    {
        CollideMap = GameObject.FindGameObjectWithTag("CollideMap").GetComponent<CompositeCollider2D>();
        OneWayMap = GameObject.FindGameObjectWithTag("OneWayMap").GetComponent<CompositeCollider2D>();

        float XAxisRaw = Input.GetAxisRaw("Horizontal");
        XAxis = Mathf.Lerp(XAxis, XAxisRaw, Smoothing);
        if (XAxis < 0.1f && XAxis > -0.1f && XAxisRaw == 0)
        {
            XAxis = 0;
        }
        if (XAxis > 0.9f && XAxisRaw == 1)
        {
            XAxis = 1;
        }
        if (XAxis < -0.9f && XAxisRaw == -1)
        {
            XAxis = -1;
        }
        float YAxisRaw = Input.GetAxisRaw("Vertical");

        //Walking + Sliding
        if (SuperLeaping == false && Stunned == false && TransitionStun == false)
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
            RB.velocity = new Vector2(XAxis * MovementSpeed * SuperJumpMultiplier * SuperChargeMultiplier * StunMultiplier, RB.velocity.y);
            var Main = PS.main;
            var Emission = PS.emission;
            Main.startSpeed = Mathf.Clamp(3 * SuperJumpMultiplier * SuperChargeMultiplier,2f,3f);
            Emission.rateOverTime = Mathf.Clamp(25 * SuperJumpMultiplier * SuperChargeMultiplier, 15f, 25f);
        }
        if (TransitionStun == true)
        {
            RB.velocity = new Vector2(0, 0);
            AN.SetBool("Walking", false);
            AN.SetBool("Sliding", false);
            AN.SetBool("Stunned", false);
            AN.SetBool("SuperJumping", false);
            AN.SetBool("Transitioning", true);
            PS.Stop();
        }
        else
        {
            AN.SetBool("Transitioning", false);
        }

        //Jumping
        if (Input.GetKey(KeyCode.Space) == true && Mathf.Approximately(RB.velocity.y, 0f) == true && (GroundChecker.IsTouching(CollideMap) || GroundChecker.IsTouching(OneWayMap)))
        {
            AN.SetBool("Stunned", false);
            Stunned = false;
            if (SuperJumpTimer > 2.5)
            {
                SuperJumpTimer = 0;
                SuperJumping = true;
                AN.SetBool("SuperJumping", true);
                BC.offset = new Vector2(0f, -0.15f);
                BC.size = new Vector2(0.6f, 0.6f);
                StartCoroutine(AudioPlay(SuperJumpSound, 1f));

                if (Mathf.Abs(XAxis) > 0.75f && SuperChargeMultiplier == 1)
                {
                    RB.velocity += new Vector2(0, SuperJumpHeight);
                    //SuperLeaping = true;
                    SuperJumpMultiplier = 0.25f;
                }
                else
                {
                    RB.velocity += new Vector2(0, SuperJumpHeight);
                    SuperJumpMultiplier = 0.25f;
                }
            }
            else
            {
                RB.velocity += new Vector2(0, JumpHeight);
                StartCoroutine(AudioPlay(JumpSound, 1f));
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
            else if (Mathf.Abs(RB.velocity.x) < 1.5f)
            {
                StartCoroutine(RestoreSuperJumpSpeed());
            }
            else
            {
                SuperJumpMultiplier = 1f;
            }
            SuperLeaping = false;
            BC.offset = new Vector2(0f, -0.25f);
            BC.size = new Vector2(0.9f, 0.47f);
        }

        //Up and Down
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && Mathf.Approximately(RB.velocity.y, 0f) == true && (GroundChecker.IsTouching(CollideMap) || GroundChecker.IsTouching(OneWayMap)))
        {
            SuperJumpTimer = 0;
        }
        if (YAxisRaw == 1 && Mathf.Approximately(RB.velocity.y, 0f) == true && (GroundChecker.IsTouching(CollideMap) || GroundChecker.IsTouching(OneWayMap)))
        {
            Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y + CameraOffset + 1.5f, 5f * Time.deltaTime), -10);
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
        if (YAxisRaw == -1 && Mathf.Approximately(RB.velocity.y, 0f) == true && (GroundChecker.IsTouching(CollideMap) || GroundChecker.IsTouching(OneWayMap)))
        {
            Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y + CameraOffset - 1.5f, 5f * Time.deltaTime), -10);
            SuperChargeMultiplier = 0.25f;
        }
        if (YAxisRaw == 0)
        {
            if (SuperJumping == true)
            {
                Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y + CameraOffset, 5f * Time.deltaTime), -10);
            }
            else
            {
                Cam.transform.position = new Vector3(Cam.transform.position.x, Mathf.Lerp(Cam.transform.position.y, gameObject.transform.position.y + CameraOffset, 7.5f * Time.deltaTime), -10);
            }
            SuperChargeMultiplier = 1f;
        }

        //Misc
        RB.velocity = new Vector2(RB.velocity.x, Mathf.Clamp(RB.velocity.y, -25f, 25f));
        AN.SetFloat("YVelocity", Mathf.RoundToInt(RB.velocity.y));
        if (Mathf.Approximately(RB.velocity.y, 0f) == false)
        {
            PS.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (CamComponent.orthographicSize == 4)
            {
                CamComponent.orthographicSize = 6;
            }
            else if (CamComponent.orthographicSize == 6)
            {
                CamComponent.orthographicSize = 4;
            }
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
                StartCoroutine(AudioPlay(ChargedSound, 0.75f));
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

    IEnumerator AudioPlay(AudioClip AC, float Volume)
    {
        AS.PlayOneShot(AC,Volume);
        yield return new WaitForSeconds(0);
    }

    public IEnumerator Damage(int Value,Vector3 Source)
    {
        if (Stunned == false && StunMultiplier == 1)
        {
            Stunned = true;
            AN.SetBool("Stunned", true);
            StartCoroutine(AudioPlay(HurtSound, 1f));

            Health -= Value; //Needs a proper death animation ------------------------------------------------------------------------------------------- !!!
            if (Health <= 0)
            {
                GM.RespawnLoad();
            }

            RB.velocity = (gameObject.transform.position - Source) * KnockbackForce + new Vector3(0, KnockbackForce);
            if (RB.velocity.x < 0)
            {
                SR.flipX = true;
                PS.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                SR.flipX = false;
                PS.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            yield return new WaitForSeconds(0.5f);
            int Timer = 0;
            while (Timer < 15 && !(GroundChecker.IsTouching(CollideMap) || GroundChecker.IsTouching(OneWayMap)))
            {
                Timer += 1;
                yield return new WaitForSeconds(0.1f);
            }
            AN.SetBool("Stunned", false);
            Stunned = false;
        }
    }

    public void Bounce(Vector2 Force)
    {
        RB.velocity = Force;
    }
}

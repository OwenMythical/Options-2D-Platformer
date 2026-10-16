using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxDetection : MonoBehaviour
{
    public int Health;
    public int ScoreOnDeath;
    public GameObject Hitbox;
    public GameObject Parent;
    public ParticleSystem PS;
    bool Cooldown;
    SpriteRenderer SR;
    Rigidbody2D RB;
    BoxCollider2D BC;
    RedSlimeMovement RSM;
    GreenSlimeMovement GSM;
    AudioSource AS;
    GameObject GameController;
    GameManager GM;

    public AudioClip HurtSound;
    public AudioClip DeathSound;

    void Start()
    {
        SR = Parent.GetComponent<SpriteRenderer>();
        RB = Parent.GetComponent<Rigidbody2D>();
        BC = Parent.GetComponent<BoxCollider2D>();
        RSM = Parent.GetComponent<RedSlimeMovement>();
        GSM = Parent.GetComponent<GreenSlimeMovement>();
        AS = gameObject.GetComponent<AudioSource>();
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GM = GameController.GetComponent<GameManager>();
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Cooldown == false)
        {
            Cooldown = true;
            Hitbox.SetActive(false);
            collision.gameObject.GetComponent<PlayerController>().Bounce(new Vector2(0,5));
            Health -= 1;
            if (GSM != null)
            {
                GSM.XSpeed = GSM.XSpeed / 2;
                GSM.CurrentSpeed = GSM.XSpeed;
            }
            StartCoroutine(AudioPlay(HurtSound, 1f));
            if (Health <= 0)
            {
                if (RSM != null)
                {
                    RSM.enabled = false;
                    RSM.Dead = true;
                }
                if (GSM != null)
                {
                    GSM.enabled = false;
                    GSM.Dead = true;
                }
                BC.enabled = false;
                GM.ScoreChange(ScoreOnDeath);
                StartCoroutine(AudioPlay(DeathSound, 1f));
                byte R = (byte)(SR.color.r * 150);
                byte G = (byte)(SR.color.g * 150);
                byte B = (byte)(SR.color.b * 150);
                RB.velocity = new Vector2(0,3);
                PS.Stop();
                byte A = 255;
                for (int i = 1; i < 10; i++)
                {
                    A -= 25;
                    yield return new WaitForSeconds(0.05f);
                    SR.color = new Color32(R, G, B, A);
                }
                GameObject.Destroy(Parent);
            }
            yield return new WaitForSeconds(0.5f);
            Hitbox.SetActive(true);
            Cooldown = false;
        }
    }
    IEnumerator AudioPlay(AudioClip AC, float Volume)
    {
        AS.pitch = Volume + UnityEngine.Random.Range(-0.05f, 0.05f);
        AS.PlayOneShot(AC, Volume);
        yield return new WaitForSeconds(0);
    }
}

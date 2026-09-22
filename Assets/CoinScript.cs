using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinScript : MonoBehaviour
{
    public int Value;
    bool Collected = false;

    GameObject Player;
    CollectableManager CM;
    Animator AN;
    Rigidbody2D RB;
    SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CM = Player.GetComponent<CollectableManager>();
        AN = gameObject.GetComponent<Animator>();
        RB = gameObject.GetComponent<Rigidbody2D>();
        SR = gameObject.GetComponent<SpriteRenderer>();
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Collected == false)
        {
            byte R = (byte)(SR.color.r * 150);
            byte G = (byte)(SR.color.g * 150);
            byte B = (byte)(SR.color.b * 150);
            Collected = true;
            CM.CoinCollect(Value);
            AN.speed = 5;
            RB.bodyType = RigidbodyType2D.Dynamic;
            RB.velocity = new Vector2(0,3);
            byte A = 255;
            for (int i = 1; i < 10; i++)
            {
                A -= 25;
                yield return new WaitForSeconds(0.05f);
                SR.color = new Color32(R,G,B,A);
            }
            GameObject.Destroy(gameObject);
        }
    }
}

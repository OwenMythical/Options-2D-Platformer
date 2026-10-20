using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDetection : MonoBehaviour
{
    public int Knockback;
    public GameObject Hurtbox;

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Hurtbox.SetActive(false);
            StartCoroutine(collision.gameObject.GetComponent<PlayerController>().Damage(1, gameObject.transform.position, Knockback));
            yield return new WaitForSeconds(0.5f);
            Hurtbox.SetActive(true);
        }
    }
}

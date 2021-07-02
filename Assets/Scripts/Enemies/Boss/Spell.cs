using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    int damage = 1;
    Player_Controler player;
    [SerializeField]AudioSource spellSound;

    void Start()
    {
        StartCoroutine(LifeTime());
        spellSound.Play();
    }

   IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player_Controler>();
        if (player != null)
        {
            player.TakeDamage(damage, 0);
        }
    }
}

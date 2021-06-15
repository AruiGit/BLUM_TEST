using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]int value = 1;
    Animator animator;
    CircleCollider2D collider;
    [SerializeField] AudioSource collectSound;
    bool isTaken = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        collectSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collectSound.Play();
            if (gameObject.CompareTag("Coin")&&isTaken==false)
            {
                collider.enabled = false;
                isTaken = true;
                collision.GetComponent<Player_Controler>().AddCoints(value);
                
                StartCoroutine(AnimationTime());
            }

            if (gameObject.CompareTag("Heart") && isTaken == false)
            {
                isTaken = true;
                collision.GetComponent<Player_Controler>().ChangeHealth(value);
                collider.enabled = false;
                animator.SetTrigger("pickedHeart");
                StartCoroutine(AnimationTime());
            }

            
        }
    }

    IEnumerator AnimationTime()
    {
        yield return new WaitForSeconds(0.517f);
        Destroy(this.gameObject);
    }
}

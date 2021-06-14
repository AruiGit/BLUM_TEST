using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]int value = 1;
    Animator animator;
    CircleCollider2D collider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (gameObject.CompareTag("Coin"))
            {
                collision.GetComponent<Player_Controler>().AddCoints(value);
                Destroy(this.gameObject);
            }

            if (gameObject.CompareTag("Heart"))
            {
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

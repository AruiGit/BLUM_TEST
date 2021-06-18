using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    protected Animator animator;
    CircleCollider2D collider;
    public AudioSource collectSound;

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
            OnCollect(collision.GetComponent<Player_Controler>());
        }
    }

    public virtual void OnCollect(Player_Controler player)
    {
        collider.enabled = false;

    }

    public IEnumerator AnimationTime(float value)
    {
        yield return new WaitForSeconds(value);
        Destroy(this.gameObject);
    }
}

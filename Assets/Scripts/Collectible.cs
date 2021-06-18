using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    protected Animator animator;
    CircleCollider2D collider;
    public AudioSource collectSound;
    protected Player_Controler player;

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
            player = collision.GetComponent<Player_Controler>();
            collectSound.Play();
            OnCollect();
        }
    }

    public virtual void OnCollect()
    {
        collider.enabled = false;

    }

    public IEnumerator AnimationTime(float value)
    {
        yield return new WaitForSeconds(value);
        Destroy(this.gameObject);
    }
}

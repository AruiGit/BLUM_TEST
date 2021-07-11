using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    int direction = -1;
    float speed = 25f;
    int damage = 2;
    SpriteRenderer sprite;
    [SerializeField] ParticleSystem explosion;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (transform.position.x < 50)
        {
            direction = 1;
            sprite.flipX = true;
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0.3f, 0);
            explosion.transform.localPosition = new Vector2(0.371f, 0f);
        }
        StartCoroutine(LifeTime());
    }

    void Update()
    {
        transform.Translate(new Vector2(direction * speed * Time.deltaTime, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            explosion.Play();
            collision.gameObject.GetComponent<Player_Controler>().TakeDamage(damage, direction);
            sprite.enabled = false;
        }
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(20);
        Destroy(this.gameObject);
    }
}

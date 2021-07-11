using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evil_Wizard_Clone : MonoBehaviour
{
    public SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is touching me");
            sprite.enabled = false;
        }
    }
    public void Life(float lifeTime)
    {
        StartCoroutine(LifeTime(lifeTime));
    }
    public IEnumerator LifeTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        sprite.enabled = false;
    }
}

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
            sprite.enabled = false;
        }
    }
}

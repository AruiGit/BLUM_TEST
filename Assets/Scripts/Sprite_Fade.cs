using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Fade : MonoBehaviour
{
    SpriteRenderer sprite;
    void Start()
    {
        sprite=GetComponent<SpriteRenderer>();
        StartCoroutine(FadeAway(0, 1));
    }

   IEnumerator FadeAway(float fadeTo, float fadeTime)
    {
        float alpha = sprite.material.color.a;
        for(float t=0.0f;t<1.0f;t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, fadeTo, t));
            sprite.material.color = newColor;
            yield return null;
        }
    }
}

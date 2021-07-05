using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Fade : MonoBehaviour
{
    SpriteRenderer sprite;
    float currentAlpha;
    void Start()
    {
        sprite=GetComponent<SpriteRenderer>();
        StartCoroutine(FadeAway(0, 1));
    }

   IEnumerator FadeAway(float fadeTo, float fadeTime)
    {
        float alpha = sprite.material.color.a;
        for(float t=0.0f;t<=1.0f;t += Time.deltaTime / fadeTime)
        {
            currentAlpha = Mathf.Lerp(alpha, fadeTo, t);
            Color newColor = new Color(1, 1, 1, currentAlpha);
            sprite.material.color = newColor;
            yield return null;
        }

        yield return new WaitForSeconds(fadeTime);
        Destroy(this.gameObject);
    }
}

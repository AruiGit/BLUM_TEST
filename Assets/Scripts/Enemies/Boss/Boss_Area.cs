using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Area : MonoBehaviour
{
    [SerializeField] GameObject bossUI;
    [SerializeField] AudioSource bossMusic;
    [SerializeField] AudioSource themeMusic;
    void Start()
    {
        bossUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bossUI.SetActive(true);
            bossMusic.Play();
            themeMusic.Stop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bossUI.SetActive(false);
            bossMusic.Stop();
            themeMusic.Play();
        }
    }
}

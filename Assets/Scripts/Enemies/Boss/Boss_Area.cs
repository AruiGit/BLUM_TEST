using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Area : MonoBehaviour
{
    [SerializeField] GameObject bossUI;
    [SerializeField] AudioSource bossMusic;
    [SerializeField] AudioSource themeMusic;
    bool isBossDead = false;
    void Start()
    {
        bossUI.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isBossDead == false) 
        {
            bossUI.SetActive(true);
            bossMusic.Play();
            themeMusic.Stop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isBossDead == false) 
        {
            bossUI.SetActive(false);
            bossMusic.Stop();
            themeMusic.Play();
        }
    }

    public void BossDead()
    {
        bossUI.SetActive(false);
        bossMusic.Stop();
        themeMusic.Play();
        isBossDead = true;
    }
}


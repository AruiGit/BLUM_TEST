using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    public Animator anim;
    AsyncOperation async;
    Player_Controler player;
    [SerializeField] AudioSource menuClickSound;


    private void Start()
    {
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;

        player = GameObject.Find("Player").GetComponent<Player_Controler>();
        player.enabled = false;
    }
    public void StartGame()
    {
        StartCoroutine(AnimationTime());
        anim.SetTrigger("playSelected");
        menuClickSound.Play();
    }

    public void OverPlayButton()
    {
        anim.SetBool("isOverPlay", true);
    }

    public void ExitPlayButton()
    {
        anim.SetBool("isOverPlay", false);
    }

    public void LoadGame()
    {
        Debug.Log("Tutaj za³aduje gre");
        menuClickSound.Play();
        player.LoadPlayer();
    }

    public void ExitGame()
    {
        menuClickSound.Play();
        Application.Quit();
    }

    IEnumerator AnimationTime()
    {
        yield return new WaitForSeconds(2.12f);
        async.allowSceneActivation = true;
    }
}

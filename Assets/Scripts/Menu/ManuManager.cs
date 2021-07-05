using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManuManager : MonoBehaviour
{
    public Animator anim;
    AsyncOperation async;
    Player_Controler player;


    private void Start()
    {
        async = SceneManager.LoadSceneAsync(0);
        async.allowSceneActivation = false;

        player = GameObject.Find("Player").GetComponent<Player_Controler>();
        player.enabled = false;
    }
    public void StartGame()
    {
        StartCoroutine(AnimationTime());
        anim.SetTrigger("playSelected");
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
        player.LoadPlayer();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator AnimationTime()
    {
        yield return new WaitForSeconds(2.12f);
        async.allowSceneActivation = true;
    }
}

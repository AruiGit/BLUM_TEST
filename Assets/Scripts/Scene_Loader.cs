using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Loader : MonoBehaviour
{
    public int nextSceneID;
    public bool wasGameLoaded;
    [SerializeField] GameObject Enemies;
    [SerializeField] GameObject Collectibles;
    [SerializeField] GameObject Doors;
    [SerializeField] GameObject shroomPrefab, goblinPrefab, deathBringerPrefab;

    private void Awake()
    {
        wasGameLoaded = GameObject_Manager.instance.wasGameLoaded;
        if (wasGameLoaded == true)
        {
            foreach(Transform enemy in Enemies.transform)
            {
                Destroy(enemy.gameObject);
            }
            foreach(Transform collectible in Collectibles.transform)
            {
                Destroy(collectible.gameObject);
            }
            Destroy(Doors);

            foreach(Enemies_Data enemy in GameObject_Manager.instance.data.enemiesToLoad)
            {
                if (enemy.typeId == 0)
                {
                    GameObject newEnemy = Instantiate(shroomPrefab, new Vector2(enemy.position[0], enemy.position[1]), Quaternion.identity);
                }
                else if (enemy.typeId == 1)
                {
                    GameObject newEnemy = Instantiate(goblinPrefab, new Vector2(enemy.position[0], enemy.position[1]), Quaternion.identity);
                }
                else if (enemy.typeId == 2)
                {
                    GameObject newEnemy = Instantiate(deathBringerPrefab, new Vector2(enemy.position[0], enemy.position[1]), Quaternion.identity);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject_Manager.instance.wasGameLoaded = false;
            GameObject_Manager.instance.bossArea = null;
            SceneManager.LoadScene(nextSceneID);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Evil_Wizard : Enemy
{
    //UI
    [SerializeField] Boss_Area bossArea;
    [SerializeField] Slider chargeAttackSlider;

    //Stats
    float spellCooldown;

    //Skills
    [SerializeField] List<GameObject> cloneSpawnPoints = new List<GameObject>();
    [SerializeField] GameObject cloneSpawnPointsParent;
    int clonesNumber = 5;
    IEnumerator chargedAttack;
    float chargeTime = 10f;

    protected override void Start()
    {
        //base.Start();
        foreach(Transform child in cloneSpawnPointsParent.transform)
        {
            cloneSpawnPoints.Add(child.gameObject);
        }
        ShuffleList();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CloneAttack();
        }
        if (playerSeen == false)
        {
            return;
        }
    }

    void CloneAttack()
    {
        transform.position = cloneSpawnPoints[0].transform.position;
        for(int i = 1; i <= clonesNumber; i++)
        {
            cloneSpawnPoints[i].GetComponent<Evil_Wizard_Clone>().sprite.enabled = true;
        }
        ShuffleList();
        chargedAttack = CharginAttack(chargeTime);
        StartCoroutine(chargedAttack);
    }
    void ShuffleList()
    {
        for (int i = 0; i < cloneSpawnPoints.Count; i++)
        {
            GameObject temp = cloneSpawnPoints[i];
            int randomIndex = Random.Range(i, cloneSpawnPoints.Count);
            cloneSpawnPoints[i] = cloneSpawnPoints[randomIndex];
            cloneSpawnPoints[randomIndex] = temp;
        }
    }

    IEnumerator CharginAttack(float chargeTime)
    {
        chargeAttackSlider.enabled = true;
        chargeAttackSlider.maxValue = chargeTime;
        float currentValue = 0;
        while (true)
        {
            currentValue = currentValue + Time.deltaTime;
            chargeAttackSlider.value = currentValue;
            float tempTime = chargeTime;
            yield return new WaitForSeconds(chargeTime);
            //Cast skill;
        }
        
    }
}

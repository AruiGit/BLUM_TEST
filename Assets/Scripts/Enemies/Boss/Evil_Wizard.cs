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

    //Clone Attack
    [SerializeField] List<GameObject> cloneSpawnPoints = new List<GameObject>();
    [SerializeField] GameObject cloneSpawnPointsParent;
    [SerializeField] GameObject fireBallPrefab;
    [SerializeField] List<Transform> fireBallSpawnPositions = new List<Transform>();
    int clonesNumber = 5;
    IEnumerator chargedAttack;
    float chargeTime = 10f;
    float cloneCooldown = 20f;
    bool isCloneAttackOnCooldown = false;
    bool isCasting = false;

    protected override void Start()
    {
        player = GameObject_Manager.instance.player.GetComponent<Player_Controler>();
        enemyAnimator = GetComponentInChildren<Animator>();
        hpBar.maxValue = healthPoints;
        hpBar.value = healthPoints;
        foreach (Transform child in cloneSpawnPointsParent.transform)
        {
            cloneSpawnPoints.Add(child.gameObject);
        }
        ShuffleList();
        chargeAttackSlider.maxValue = chargeTime;
    }

    protected override void Update()
    {
        if (playerSeen == false)
        {
            return;
        }
        NormalAttack();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isCloneAttackOnCooldown == false)
        {
            isCloneAttackOnCooldown = true;
            CloneAttack();
            StartCoroutine(CloneCooldown(cloneCooldown));
        }
    }
    void CloneAttack()
    {
        transform.position = cloneSpawnPoints[0].transform.position;
        enemyAnimator.SetTrigger("startingCast");
        for(int i = 1; i <= clonesNumber; i++)
        {
            cloneSpawnPoints[i].GetComponentInChildren<Evil_Wizard_Clone>().sprite.enabled = true;
            cloneSpawnPoints[i].GetComponentInChildren<Evil_Wizard_Clone>().Life(chargeTime);
        }
        ShuffleList();

        isCasting = true;
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
    void SpawnFireBalls()
    {
        foreach(Transform fireball in fireBallSpawnPositions)
        {
            Instantiate(fireBallPrefab, fireball.position, Quaternion.identity);
        }
    }
    void NormalAttack()
    {
        if(Mathf.Abs(player.transform.position.x - transform.position.x) < 3 && player.transform.position.y - transform.position.y < 3 && isCasting == false)
        {
            enemyAnimator.SetFloat("direction", player.transform.position.x - transform.position.x);
            enemyAnimator.SetTrigger("isAttacking");
        }
    }

    public override void TakeDamage(int value, int direction)
    {
        if (canTakeDamage == true)
        {
            canTakeDamage = false;
            healthPoints -= value;
            enemyAnimator.SetTrigger("isHit");
            hpBar.value = healthPoints;
            StartCoroutine(TakeDamage());
        }
        enemyAnimator.SetBool("isCasting", false);
        StopCoroutine(chargedAttack);
        chargeAttackSlider.value = 0;
        transform.position = cloneSpawnPoints[0].transform.position;
        isCasting = false;
        ShuffleList();
    }

    IEnumerator CharginAttack(float chargeTime)
    {
        enemyAnimator.SetBool("isCasting", true);
        for (float t = 0.0f; t <= chargeTime+0.1f; t += Time.deltaTime)
        {
            chargeAttackSlider.value = t;
            yield return null;
        }
        SpawnFireBalls();
        enemyAnimator.SetBool("isCasting", false);
        enemyAnimator.SetTrigger("finishedCasting");
        chargeAttackSlider.value = 0;
        isCasting = false;
    }
    IEnumerator CloneCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        isCloneAttackOnCooldown = false;
    }
}

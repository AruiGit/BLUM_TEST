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
    [SerializeField] Transform attackPosition;
    int clonesNumber = 5;
    IEnumerator chargedAttack;
    float chargeTime = 10f;
    float cloneCooldown = 20f;
    bool isCloneAttackOnCooldown = false;
    bool isCasting = false;
    public bool isHit = false;
    bool canShoot = true;


    protected override void Start()
    {
        player = GameObject_Manager.instance.player.GetComponent<Player_Controler>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
        UpdateFlip();

        if (healthPoints <= 0)
        {
            if (isPlaying == false)
            {
                enemyAnimator.SetTrigger("isDead");
                isPlaying = true;
                dyingSound.Play();
                rb.gravityScale = 0;
                foreach (Collider2D col in enemyColliders)
                {
                    col.enabled = false;
                }
            }
            rb.velocity = new Vector2(0, 0);

            StartCoroutine(DeathTimer(0.8f));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isCloneAttackOnCooldown == false)
        {
            isCloneAttackOnCooldown = true;
            enemyAnimator.SetTrigger("startingCast");
            enemyAnimator.SetBool("isCasting", true);
            StartCoroutine(WaitForAnimationEnd(0.40f));
            StartCoroutine(CloneCooldown(cloneCooldown));
        }
    }
    void CloneAttack()
    {
        transform.position = cloneSpawnPoints[0].transform.position;
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
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 7f && Mathf.Abs(player.transform.position.y - transform.position.y) < 1.5f && isCasting == false && canShoot==true)
        {
            canShoot = false;
            StartCoroutine(CastSingleFireBall(0.5f));
            enemyAnimator.SetTrigger("isAttacking");
        }
    }
    void UpdateFlip()
    {
        enemyAnimator.SetFloat("direction", player.transform.position.x - transform.position.x);
        if (player.transform.position.x - transform.position.x < 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
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
            isHit = true;
            StartCoroutine(Hitted());
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
    IEnumerator WaitForAnimationEnd(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        CloneAttack();
    }
    IEnumerator Hitted()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }
    IEnumerator CastSingleFireBall(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        Instantiate(fireBallPrefab, attackPosition.position, Quaternion.identity);
        canShoot = true;
    }
}

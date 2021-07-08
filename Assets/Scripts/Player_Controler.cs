using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controler : MonoBehaviour
{
    bool isDead = false;
    private static Player_Controler playerInstance;

    //Sprite and animations
    [SerializeField]SpriteRenderer sprite;
    [SerializeField]Animator playerAnimator;
    [SerializeField]GameObject jumpFadeSprite;
    Quaternion jumpFadeSpriteRotation = new Quaternion();

    //Movement
    Rigidbody2D rb;
    int speed = 5;
    int dir = 1;
    float horizontalInput;
    float maxJumpHight = 10;
    float minimumJumpHight = 3f;
    float jumpHight;
    bool isGrounded;
    bool isPreparingToJump = false;
    RaycastHit2D ray, rayHead;
    Collider2D collider;

    //Dash
    float dashLenght = 3;
    bool canDash = true;
    [SerializeField]ParticleSystem jumpParticle;

    //Stats
    [SerializeField]int healthPoints = 3;
    public int maxHealthPoints = 3;
    int money = 540;
    int damage = 1;

    //Attack
    [SerializeField]Transform attackPosition;
    [SerializeField]float attackRange = 0.5f;
    bool canAttack = true;
    bool isAttacking = false;
    bool damageDealt = false;
    AudioSource attackSound;

    //TakingDamage
    bool canTakeDamage = true;
    bool isColliding = false;

    //Envo
    int secretKeys = 0;
    [SerializeField]Camera_Movement camera;

    //Unlocks
    bool isDashUnlocked = true;

    void Awake()
    {
        Debug.Log("im alive!");
        if (playerInstance == null)
        {
            playerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(playerInstance != this)
        {
            Destroy(gameObject);
        }
        GameObject_Manager.instance.player = this.gameObject;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackSound = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        if (camera == null)
        {
            camera = GameObject_Manager.instance.camera.GetComponent<Camera_Movement>();
        }
        if (GameObject_Manager.instance.player == null)
        {
            GameObject_Manager.instance.player = this.gameObject;
        }
        if (isDead == false)
        {
            Movement();
            Jump();
            Attack();
            Dash();
        }

        if (healthPoints <= 0)
        {
            isDead = true;
            playerAnimator.SetBool("isDead 0",true);
            StartCoroutine(deathTimer());
            collider.enabled = false;
            rb.gravityScale = 0;
        }
    }

    private void OnDestroy()
    {
        GameObject_Manager.instance.player = null;
    }
    #region Movement
    void Movement()
    {
        if (isPreparingToJump == false && isColliding == false)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            Vector2 direction = new Vector2(horizontalInput, 0);
            playerAnimator.SetBool("isRuning", true);
            ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.45f), direction, 0.6f, LayerMask.GetMask("Map"));
            rayHead = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.45f), direction, 0.6f, LayerMask.GetMask("Map"));

            if (ray.collider != null || rayHead.collider != null)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                return;
            }
            if (horizontalInput != 0)
            {
                rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
            }
            if (horizontalInput < 0 && isAttacking == false)
            {
                sprite.flipX = true;
                playerAnimator.SetBool("isFlipped", true);
                attackPosition.localPosition = new Vector2(-0.135f, 0);
                dir = -1;
            }
            else if (horizontalInput > 0 && isAttacking == false)
            {
                playerAnimator.SetBool("isFlipped", false);
                sprite.flipX = false;
                attackPosition.localPosition = new Vector2(0.135f, 0);
                dir = 1;
            }
        }
        if (horizontalInput == 0)
        {
            playerAnimator.SetBool("isRuning", false);
        }
        if (isColliding == true)
        {
            playerAnimator.SetBool("isRuning", false);
        }
    }
    void Jump()
    {
        if (isGrounded == true)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jumpParticle.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpHight);
                playerAnimator.ResetTrigger("prepToJump");
                playerAnimator.SetBool("isJumping", true);
                isGrounded = false;
                jumpHight = 0;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                jumpHight += 10 * Time.deltaTime;
                jumpHight = Mathf.Clamp(jumpHight, minimumJumpHight, maxJumpHight);
                playerAnimator.SetTrigger("prepToJump");
                isPreparingToJump = true;
            }
            else
            {
                isPreparingToJump = false;
            }
            if (isGrounded == true)
            {
                playerAnimator.SetBool("isJumping", false);
            }
        }
        if (isGrounded == false)
        {
            isPreparingToJump = false;
        }

        if (rb.velocity.y < -1f)
        {
            playerAnimator.SetBool("isFalling", true);
        }
        else
        {
            playerAnimator.SetBool("isFalling", false);
        }
    }
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isDashUnlocked == true)
        {
            int layerMask = ~LayerMask.GetMask("Player");
            ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.45f), new Vector2(dir, 0), dashLenght, layerMask);
            rayHead = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.45f), new Vector2(dir, 0), dashLenght, layerMask);
            Debug.Log(ray.collider);
            if (ray.collider == null && rayHead.collider == null && canDash == true)
            {
                if (dir < 0)
                {
                    jumpFadeSpriteRotation.Set(0, 180, 0, 0);
                }
                else
                {
                    jumpFadeSpriteRotation.Set(0, 0, 0, 1);
                }
                StartCoroutine(SpawnDashFade(transform.position));

                gameObject.transform.Translate(new Vector2(dashLenght * dir, 0));
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
                canDash = false;
                StartCoroutine(dashCooldown());
                StartCoroutine(camera.CameraShake(0.1f, 0.2f));
            }
        }
    }
    #endregion
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.E) && canAttack==true)
        {
            attackSound.Play();
            isAttacking = true;
            Collider2D[] enemiesArrey = Physics2D.OverlapCircleAll(attackPosition.position, attackRange,LayerMask.GetMask("Enemy"));
            playerAnimator.SetTrigger("isAttacking");
            canAttack = false;
            StartCoroutine(attackCooldown());

            foreach(Collider2D enemy in enemiesArrey)
            {
                if (enemy.CompareTag("Death_Bringer") && damageDealt==false)
                {
                    damageDealt = true;
                    enemy.gameObject.GetComponent<Death_Bringer>().TakeDamage(damage, dir);
                }
                else if (damageDealt == false)
                {
                    damageDealt = true;
                    enemy.gameObject.GetComponent<Enemy>().TakeDamage(damage, dir);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPosition == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
    public void TakeDamage(int value, int direction)
    {
        HealthPoints = -value;
        rb.AddForce(new Vector2(125 * direction, 0));
    }
    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Enemy") && isColliding == false)
        {
            if (collision.gameObject.GetComponent<Shroom>() != null)
            {
                collision.gameObject.GetComponent<Shroom>().CrushDamage(damage);
                rb.velocity = new Vector2(rb.velocity.x, 7);
                HealthPoints = 0;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 7);
                HealthPoints = -1;
            }
            isColliding = true;
            StartCoroutine(StunTime(0.2f));
        }
        if (collision.gameObject.CompareTag("Death_Bringer"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 15);
            HealthPoints = -1;
            isColliding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Death_Bringer"))
        {
            isColliding = false;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Spike"))
        {
            isColliding = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isColliding == true)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Death_Bringer"))
        {
            HealthPoints = -1;
            Vector2 dir = transform.position - collision.transform.position;
            if (dir.x > 0)
            {
                rb.velocity=new Vector2(7, 0);
            }
            else
            {
                rb.velocity=new Vector2(-7, 0);
            }
            isColliding = true;
            StartCoroutine(StunTime(0.2f));
        }
        if(collision.gameObject.CompareTag("Spike"))
        {
            HealthPoints = -1;
            isColliding = true;
        }
    }
    public void ChangeCollision()
    {
        isColliding = !isColliding;
    }
    #endregion
    public bool CheckDeath()
    {
        return isDead;
    }
    public void DestroyPlayer()
    {
        playerInstance = null;
        Destroy(gameObject);
    }
    #region Properties
    public int SecretKeys
    {
        get
        {
            return secretKeys;
        }
        set
        {
            secretKeys += value;
        }
    }
    public int HealthPoints
    {
        get
        {
            return healthPoints;
        }
        set 
        {
            if (value <= 0)
            {
                if (canTakeDamage == true)
                {
                    healthPoints += value;
                    canTakeDamage = false;
                    StartCoroutine(TakeDamage());
                }
            }
            else
            {
                healthPoints += value;
            }
            if (healthPoints > maxHealthPoints)
            {
                healthPoints = maxHealthPoints;
            }
        }
    }
    public int Coins
    {
        get 
        {
            return money;
        }
        set
        {
            money += value;
        }
    }
    public int MaxHealthPoints
    {
        get
        {
            return maxHealthPoints;
        }
        set
        {
            maxHealthPoints += value;
        }
    }
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage += value;
        }
    }
    public bool IsDashUnlocked
    {
        get
        {
            return isDashUnlocked;
        }
        set
        {
            isDashUnlocked = value;
        }
    }
    public Vector3 GetPlayerPosition()
    {
        return gameObject.transform.position;
    }
    public int GetActiveSceneID()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    #endregion
    #region Save/Load
    public void SavePlayer()
    {
        Save_System.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        Save_Data data = Save_System.LoadPlayer();

        GameObject_Manager.instance.data = data;
        ReloadPlayer(data);
        SceneManager.LoadScene(data.playerToSave.sceneID);
        this.enabled = true;
    }

    public void ReloadPlayer(Save_Data data)
    {
        money = data.playerToSave.money;
        healthPoints = data.playerToSave.health;
        maxHealthPoints = data.playerToSave.maxHealth;
        damage = data.playerToSave.damage;
        secretKeys = data.playerToSave.keys;
        transform.position = new Vector3(data.playerToSave.position[0], data.playerToSave.position[1], data.playerToSave.position[2]);
        isDashUnlocked = data.playerToSave.isDashUnlocked;
    }
    #endregion
    IEnumerator TakeDamage()
    {
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }
    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(0.517f);
        canAttack = true;
        isAttacking = false;
        damageDealt = false;
        playerAnimator.ResetTrigger("isAttacking");
    }
    IEnumerator deathTimer()
    {
        yield return new WaitForSeconds(0.517f);
        sprite.enabled = false;
    }
    IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(2f);
        canDash = true;
    }
    public IEnumerator StunTime(float time)
    {
        yield return new WaitForSeconds(time);
        isColliding = false;
    }
    IEnumerator SpawnDashFade(Vector3 tempPlayerPos)
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(jumpFadeSprite, new Vector2(tempPlayerPos.x, tempPlayerPos.y), jumpFadeSpriteRotation);
        yield return new WaitForSeconds(0.05f);
        Instantiate(jumpFadeSprite, new Vector2(tempPlayerPos.x + dashLenght / 4 * dir, tempPlayerPos.y), jumpFadeSpriteRotation);
        yield return new WaitForSeconds(0.05f);
        Instantiate(jumpFadeSprite, new Vector2(tempPlayerPos.x + dashLenght * 2 / 4 * dir, tempPlayerPos.y), jumpFadeSpriteRotation);
        yield return new WaitForSeconds(0.05f);
        Instantiate(jumpFadeSprite, new Vector2(tempPlayerPos.x + dashLenght * 3 / 4 * dir, tempPlayerPos.y), jumpFadeSpriteRotation);
        yield return new WaitForSeconds(0.05f);
        Instantiate(jumpFadeSprite, new Vector2(tempPlayerPos.x + dashLenght * dir, tempPlayerPos.y), jumpFadeSpriteRotation);
    }
}

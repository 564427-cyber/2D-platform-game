using System.Collections;
using UnityEditor.Build;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
   

    //boss settings
    public float maxHealth = 200f;
    private float currentHealth;

    //detection settings
    public float detectionRange = 15f; //how far away before the boss can detect and chase the player
    private bool playerDetected = false;
    private float timeSincePlayerDetected = 0f;
    public float attackDelay = 3f; //how long the boss can attack after detecting a player

    //Attack settings
    public float attackRange = 5f;
    public float attackDamage = 10f;
    public float attackCooldown = 5f;
    private float lastAttackTime;
    private bool isRecovering = false;
    public float recoveryTime = 3f; //how long the boss will pause after attacking

    //Movement settings
    public float moveSpeed = 3f;
    public Transform player;

    private Animator animator;

    //Health bar UI
    public Slider healthBar;
    public GameObject healthBarUI; // the parent gameobject (slider) to show / hide

   
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (healthBar != null)
            healthBar.maxValue = maxHealth;

        if (healthBarUI != null) 
            healthBarUI.SetActive(false);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Player object not found! Make sure it’s tagged 'Player'.");





    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        //check if player is within detection range
        if (distance <= detectionRange)
        {
            if (!playerDetected)
            {
                playerDetected = true;
                if (healthBarUI != null)
                    healthBarUI.SetActive(true);
                timeSincePlayerDetected = 0f; //reset timer when first detecting player
            }
            else
            {
                timeSincePlayerDetected += Time.deltaTime; //count up while player is detected
            }
        }
        else if (distance > detectionRange * 1.2f)
        {
            playerDetected = false;
            if (healthBarUI != null)
                healthBarUI.SetActive(false);
            timeSincePlayerDetected = 0f; //reset when losing player
        }
        if (!playerDetected)
        {
            animator?.SetBool("isMoving", false);
            return;
        }


        //face the player
        Vector3 lookDir = player.position - transform.position;

        if (lookDir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); //face right
        }
        else if (lookDir.x < 0)
        {
            transform.localScale = new Vector3(-1, -1, -1); //face left
        }
        //Move towards player if out of attack range
        if(distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else if (timeSincePlayerDetected >= attackDelay)
        {
            AttackPlayer(); //only attack after delay
        }
        else
        {
            MoveTowardsPlayer(); //keep moving closer until delay passes
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        animator?.SetBool("isMoving", true);
    }

    void AttackPlayer()
    {
        Debug.Log("AttackPlayer() called");

        if (isRecovering) return;

        animator.SetTrigger("Attack");

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Player in range — trying to deal damage");

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Dealt " + attackDamage + " damage");
            }
        }

        lastAttackTime = Time.time;
        StartCoroutine(RecoverAfterAttack());
    }



    IEnumerator PlayAttackAnimation()
    {
        animator?.SetTrigger("attack");
        yield return new WaitForSeconds(2f); // Delay before next animation 
        animator?.ResetTrigger("attack");
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.value = currentHealth; //updates the slider instantly
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    
    void Die()
    {
         Debug.Log("Boss defeated!");
         animator?.SetTrigger("die");
         this.enabled = false;

         if (healthBarUI != null)
            healthBarUI.SetActive(false);

         // Stop the timer when boss dies
         LevelTimer timer = FindFirstObjectByType<LevelTimer>();
         if (timer != null)
            timer.StopTimer();

        Destroy(gameObject, 0.5f);
    }


    IEnumerator RecoverAfterAttack()
    {
        isRecovering = true;
        yield return new WaitForSeconds(recoveryTime);
        isRecovering = false;
    }

}


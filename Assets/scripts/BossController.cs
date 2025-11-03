using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{   
    //boss settings
    public float maxHealth = 150f;
    private float currentHealth;

    //detection settings
    public float detectionRange = 15f; 
    private bool playerDetected = false;
    private float timeSincePlayerDetected = 0f;
    public float attackDelay = 4f; 

    //Attack settings
    public float attackRange = 5f;
    public float attackDamage = 25f;
    public float attackCooldown = 5f;    
    private bool isRecovering = false;
    public float recoveryTime = 4f; 

    //Movement settings
    public float moveSpeed = 3f;
    public Transform player;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
            
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Player object not found! Make sure it’s tagged 'Player'.");

    }        
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
                
        if (distance <= detectionRange)
        {
            if (!playerDetected)
            {
                playerDetected = true;
                timeSincePlayerDetected = 0f; 
            }
            else
            {
                timeSincePlayerDetected += Time.deltaTime; 
            }
        }
        else if (distance > detectionRange * 1.2f)
        {
            playerDetected = false;
            timeSincePlayerDetected = 0f; 
        }
        if (!playerDetected)
        {
            return;
        }
                
        Vector3 lookDir = player.position - transform.position;

        if (lookDir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); 
        }
        else if (lookDir.x < 0)
        {
            transform.localScale = new Vector3(-1, -1, -1); 
        }
       
        if(distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else if (timeSincePlayerDetected >= attackDelay)
        {
            AttackPlayer(); 
        }
        else
        {
            MoveTowardsPlayer(); 
        }
    }
    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);        
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
        StartCoroutine(RecoverAfterAttack());
    }
   
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

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

        Destroy(gameObject, 0.5f);
    }
    IEnumerator RecoverAfterAttack()
    {
        isRecovering = true;
        yield return new WaitForSeconds(recoveryTime);
        isRecovering = false;
    }
}
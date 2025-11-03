
using UnityEngine;

public class Spritemove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rb;
    bool isGrounded;
    public Animator anim;
    LayerMask groundLayerMask;
    public int lives;
    private bool facingRight = true;
    HelperScript helper;
    public GameObject weapon;
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        lives = 3;
        currentHealth = maxHealth;
        helper = gameObject.AddComponent<HelperScript>();
    }
    // Update is called once per frame
    void Update()
    {                
        //  FlipController();
        float xvel, yvel;

        xvel = rb.linearVelocity.x;
        yvel = rb.linearVelocity.y;
        
        if (Input.GetKey("a"))
        {
            xvel = -4;
            facingRight = false;
        }
        if (Input.GetKey("d"))
        {
            xvel = 4;
            facingRight = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded )
        {
            yvel = +7;
        }
        if (xvel >= 0.1f || xvel <= -0.1f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        rb.linearVelocity = new Vector3(xvel, yvel, 0);

        shoot();

        //do raycasting check
        bool gc = ExtendedRayCollisionCheck(0, 0);
        print("gc=" + gc);

        isGrounded = gc;

        if(isGrounded == true)
        {
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
        }
        if (xvel < 0)
        {
            helper.DoFlipObject(true);
        }
        if (xvel > 0)
        {
            helper.DoFlipObject(false);
        }
    }
    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage() called with damage: " + damage);

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Health hit zero!");
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Player died!");

        if (anim != null)
            anim.SetTrigger("die");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        this.enabled = false;
    }
          

        void shoot()
        {
            if (Input.GetKeyDown("e"))
            {
                GameObject clone;
                clone = Instantiate(weapon, transform.position, transform.rotation);

                Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();

                rb.linearVelocity = new Vector2(15, 0);

                if (facingRight)
                {
                    rb.linearVelocity = new Vector2(15, 0);
                    clone.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1.25f, transform.position.z);
                }
                else
                {
                    rb.linearVelocity = new Vector2(-15, 0);
                    clone.transform.position = new Vector3(transform.position.x - 1, transform.position.y + 1.25f, transform.position.z);
                }
            }
        }            
        
    public bool ExtendedRayCollisionCheck(float xoffs, float yoffs)
    {
        float rayLength = 0.5f;
        bool hitSomething = false;

        Vector3 offset = new Vector3(xoffs, yoffs, 0);

        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position + offset, -Vector2.up, rayLength, groundLayerMask);

        Color hitColor = Color.white;

        if (hit.collider != null)
        {
            hitColor = Color.green;
            hitSomething = true;
        }

        Debug.DrawRay(transform.position + offset, -Vector3.up * rayLength, hitColor);

        return hitSomething;
    }
}
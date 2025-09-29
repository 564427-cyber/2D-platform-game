using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Spritemove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Rigidbody2D rb;
    bool isGrounded;
    public Animator anim;
    LayerMask groundLayerMask;

    private bool facingRight = true;
    
    
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        FlipController();

        float xvel, yvel;

        xvel = rb.linearVelocity.x;
        yvel = rb.linearVelocity.y;
        
        if (Input.GetKey("a"))
        {
            xvel = -2;
        }

        if (Input.GetKey("d"))
        {
            xvel = +2;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded )
        {
            yvel = +5;
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




    }


    private void Flip()
    {
        facingRight = !facingRight; //works as a switcher
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (rb.linearVelocity.x < 0 && facingRight)
            Flip();
        else if (rb.linearVelocity.x > 0 && !facingRight)
            Flip();
    }

   


    

    /*

     private void OnCollisionEnter2D(Collision2D other)
     {
         if (other.gameObject.CompareTag("floor"))
         {
             anim.SetBool("isJumping", false);
             isGrounded = true;
         }
     }

     private void OnCollisionExit2D(Collision2D other)
     {
         if (other.gameObject.CompareTag("floor"))
         {
             anim.SetBool("isJumping", true);
             isGrounded = false;
         }
     }
    */

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

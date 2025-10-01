using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Spritemove playerScript;

    LayerMask groundLayerMask;
    float xvel;
    bool isGrounded;
    Rigidbody2D rb;
    bool gc;

    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        xvel = 1;
    }

    // Update is called once per frame
    void Update()
    {

        print("Enemy says: The player has " + playerScript.lives + " lives ");

        
        isGrounded = gc;
        //if  enemy moving left, check left side of sprite

        if (xvel < 0)
        {
            gc = ExtendedRayCollisionCheck(-0.25f, 0);
        }
       
        //if  enemy moving right, check right side of sprite

        if (xvel > 0)
        {
            gc = ExtendedRayCollisionCheck(0.25f, 0);
        }


        if (gc == false)
        {
            xvel = -xvel;
        }
      

            rb.linearVelocity = new Vector2(xvel, 0);

    }


    public bool ExtendedRayCollisionCheck(float xoffs, float yoffs)
    {
        float rayLength = 0.5f; // length of raycast
        bool hitSomething = false;

        // convert x and y offset into a Vector3 
        Vector3 offset = new Vector3(xoffs, yoffs, 0);

        //cast a ray downward 
        RaycastHit2D hit;


        hit = Physics2D.Raycast(transform.position + offset, -Vector2.up, rayLength, groundLayerMask);

        Color hitColor = Color.white;


        if (hit.collider != null)
        {
            print("Player has collided with Ground layer");
            hitColor = Color.green;
            hitSomething = true;
        }
        // draw a debug ray to show ray position
        // You need to enable gizmos in the editor to see these
        Debug.DrawRay(transform.position + offset, -Vector3.up * rayLength, hitColor);

        return hitSomething;
        

        
    }






}

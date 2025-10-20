using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 5f; //how much damage the bullet does
    public float lifetime = 3f; //how long until the bullet destroys itself
    void Start()
    {
        Destroy(gameObject, lifetime); //destroy after a few seconds to avoid clutter
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        //check if we hit the boss
        if (other.CompareTag("Boss"))
        {
            BossController boss = other.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            Destroy(gameObject); //destroy bullet after hit
        }

        // stop bullets hitting player on environment
        else if (!other.CompareTag("player"))
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

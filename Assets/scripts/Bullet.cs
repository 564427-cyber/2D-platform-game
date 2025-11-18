using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; //how much damage the bullet does
    public float lifetime = 3f; //how long until the bullet destroys itself
    void Start()
    {
        Destroy(gameObject, lifetime); //destroy after a few seconds to avoid clutter
    }
    private void OnTriggerEnter2D(Collider2D other)
    {        
        //check if player hits the boss
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

        if (other.CompareTag("enemy"))
        {
            EnemyMove enemy = other.GetComponent<EnemyMove>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        else if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

    }  

   
    
}

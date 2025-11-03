using UnityEngine;
using UnityEngine.SceneManagement; // ✅ Add this!

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public Transform player;
    public Transform respawnPoint;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Player took damage! Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player is dead!");

        //restart the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


  
    



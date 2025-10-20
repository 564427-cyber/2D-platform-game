using UnityEngine;

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

        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
            health = 100f;
            Debug.Log("Player respawned!");
        }
        else
        {
            Debug.LogWarning("Respawn point not assigned");
        }

    }

  
    

}

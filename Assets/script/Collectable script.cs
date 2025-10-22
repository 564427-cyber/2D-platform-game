using UnityEngine;

public class Wall : MonoBehaviour
{    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);

        }
    }
}
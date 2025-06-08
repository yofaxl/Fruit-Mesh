using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 2;
   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damage);

        }
    } 
    private void OnTriggerEnter2D(Collider2D other)
   {
    if (other.CompareTag("Player"))
    {
        playerHealth.TakeDamage(damage);
    }
   }

}

using UnityEngine;

public class MonsterStomp : MonoBehaviour
{
    public float bounce;
    public Rigidbody2D rb2D;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyHead"))
        {
            
            GameObject enemy = other.transform.root.gameObject;
            Destroy(enemy);

           
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, bounce);
        }
    }
}

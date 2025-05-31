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
            Debug.Log("Enemy root: " + enemy.name);

            Animator anim = enemy.GetComponent<Animator>();
            if (anim == null)
            {
                anim = enemy.GetComponentInChildren<Animator>();
            }

            if (anim != null)
            {
                anim.SetTrigger("Die"); 
                Debug.Log("Die trigger set edildi.");
            }
            else
            {
                Debug.LogWarning("Animator bulunamadı!");
            }

            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, bounce);
        }
    }
}

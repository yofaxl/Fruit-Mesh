using UnityEngine;

public class MonsterStomp : MonoBehaviour
{
    public float bounce;
    public Rigidbody2D rb2D;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyHead"))
        {
            EnemyHead head = other.GetComponent<EnemyHead>();

            if (head != null && head.enemyRoot != null)
            {
                Animator anim = head.enemyRoot.GetComponent<Animator>();

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
}

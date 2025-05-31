using UnityEngine;


public class Enemy : MonoBehaviour
{
    public Collider2D attackCollider; // Hasar veren collider

    // Animation Event'ten çağrılacak
    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
            Debug.Log("Saldırı collider'ı kapatıldı.");
        }
    }
}



using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int value;
    private Animator animator;
    private bool isCollected = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            CoinCounter.instance.IncreaseCoins(value);

            if (animator != null)
            {
                animator.SetTrigger("Collected");
            }

            // Collider'ı kapat ki tekrar toplanmasın
            GetComponent<Collider2D>().enabled = false;
        }
    }

    // Animasyon sonunda çağrılacak (Animator Event ile)
    public void DestroyCoin()
    {
        Destroy(gameObject);
    }
}

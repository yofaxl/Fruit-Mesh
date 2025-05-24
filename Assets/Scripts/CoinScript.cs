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

           
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayFruitSound();
            }
            else
            {
                Debug.LogWarning("AudioManager instance is null when collecting fruit.");
            }

            if (animator != null)
            {
                animator.SetTrigger("Collected");
            }

            
            GetComponent<Collider2D>().enabled = false;
        }
    }

    
    public void DestroyCoin()
    {
        Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 10;
    private bool isImmune = false;

    private Animator animator;  
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();  
    }

    public void TakeDamage(int amount)
    {
        if (isImmune) return;

        health -= amount;

        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDamageSound();
        }
        else
        {
            Debug.LogWarning("AudioManager instance is null when player takes damage.");
        }

        if (animator != null)
        {
            animator.SetTrigger("DamageAnim");  
        }

        if (health <= 0)
        {
            
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ShowLosePanel();
            }
            else
            {
                Debug.LogError("LevelManager instance is null when player health reached zero.");
            }
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayDeathSound();
            }
            else
            {
                Debug.LogWarning("AudioManager instance is null when player dies.");
            }
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(ImmunityCoroutine());
        }
    }

    private IEnumerator ImmunityCoroutine()
    {
        isImmune = true;
        yield return new WaitForSeconds(2f);
        isImmune = false;
    }
}

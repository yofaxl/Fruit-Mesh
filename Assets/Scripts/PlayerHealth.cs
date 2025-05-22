using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 10;
    private bool isImmune = false;

    private Animator animator;  // reference to Animator

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();  // get Animator component attached to the player
    }

    public void TakeDamage(int amount)
    {
        if (isImmune) return;

        health -= amount;

        if (animator != null)
        {
            animator.SetTrigger("DamageAnim");  // trigger damage animation
        }

        if (health <= 0)
        {
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

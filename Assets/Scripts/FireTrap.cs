using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 2;

    [Header("Timing Settings")]
    [SerializeField] private float activeTime = 2f;
    [SerializeField] private float inactiveTime = 1.5f;

    private Animator anim;
    private bool isActive = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(FiretrapLoop());
    }

    private IEnumerator FiretrapLoop()
    {
        while (true)
        {
            // Aktif hale getir
            isActive = true;
            anim.SetBool("activated", true); // Aktif animasyonu çalıştır
            yield return new WaitForSeconds(activeTime);

            // Pasif hale getir
            isActive = false;
            anim.SetBool("activated", false); // Bekleme animasyonu çalıştır
            yield return new WaitForSeconds(inactiveTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}

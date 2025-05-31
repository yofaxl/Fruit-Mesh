using UnityEngine;

public class TrunkTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows; // Arrow prefablarının dizisi

    private float cooldownTimer;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        cooldownTimer = 0;

        int arrowIndex = FindArrow();
        GameObject arrowObj = arrows[arrowIndex];
        
        arrowObj.transform.position = firePoint.position;

        EnemyProjectile arrowProjectile = arrowObj.GetComponent<EnemyProjectile>();

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        arrowProjectile.SetDirection(direction);
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0; // Eğer hepsi aktifse ilkini kullan
    }
}

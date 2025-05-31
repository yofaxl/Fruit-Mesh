using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 2f;
    [SerializeField] private int damage = 1;  // Hasar miktarı

    private float lifetime;
    private float direction;
    private bool hit;

    private void Update()
    {
        if (hit) return;

        float moveDistance = speed * Time.deltaTime * direction;
        transform.Translate(moveDistance, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        hit = false;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player'da bir Health ya da PlayerHealth scripti olduğunu varsayıyoruz.
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        hit = true;
        gameObject.SetActive(false);
    }
}

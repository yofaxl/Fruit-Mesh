using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private Animator anim;
    private bool triggered = false;
    private bool levelCompleted = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            anim.SetTrigger("Pressed");
        }
    }

    // Bu fonksiyon Animation Event ile çağrılacak
    public void CompleteLevel()
    {
        if (levelCompleted) return;

        levelCompleted = true;
        Debug.Log("Level Complete çağrıldı!");
        LevelManager.Instance.LevelCompleted();

        // Tekrar tetiklenmemesi için objeyi devre dışı bırak
        gameObject.SetActive(false);
    }
}

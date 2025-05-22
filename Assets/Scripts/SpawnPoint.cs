using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int levelNumber; // Bu spawn point'in hangi level için olduğunu belirtir
    
    private void OnDrawGizmos()
    {
        // Scene view'da spawn point'i görselleştirmek için
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
    }
} 
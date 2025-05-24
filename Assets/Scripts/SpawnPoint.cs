using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int levelNumber; 
    
    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
    }
} 
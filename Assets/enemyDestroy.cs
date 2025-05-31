using UnityEngine;

public class Enemy12 : MonoBehaviour
{
public void DestroyEnemy()
{
    Destroy(transform.root.gameObject);
}
}

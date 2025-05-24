using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private HashSet<GameObject> portalObjects = new HashSet<GameObject>();

    [SerializeField] private Transform destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (portalObjects.Contains(collision.gameObject))
        {
            return;
        }

        if (destination.TryGetComponent(out Portal destinationPortal))
        {
            destinationPortal.portalObjects.Add(collision.gameObject);
        }

        collision.transform.position = destination.position;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayTeleportSound();
        }
        else
        {
            Debug.LogWarning("AudioManager instance is null when teleporting.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        portalObjects.Remove(collision.gameObject);
    }
}
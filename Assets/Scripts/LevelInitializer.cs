using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInitializer : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerInScene; 

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level"))
        {
            int levelNumber = int.Parse(sceneName.Substring(5));
            MovePlayerToSpawn(levelNumber);
        }
        else
        {
            Debug.LogError("LevelInitializer: Scene name does not follow the 'LevelX' format!");
        }
    }

    private void MovePlayerToSpawn(int levelNumber)
    {
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();

        SpawnPoint targetSpawnPoint = null;
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.levelNumber == levelNumber)
            {
                targetSpawnPoint = spawnPoint;
                break;
            }
        }

        if (targetSpawnPoint != null)
        {
            if (playerInScene != null)
            {
                playerInScene.transform.position = targetSpawnPoint.transform.position;
                playerInScene.transform.rotation = targetSpawnPoint.transform.rotation;
            }
            else
            {
                Debug.LogError("LevelInitializer: Player reference is missing!");
            }
        }
        else
        {
            Debug.LogError($"LevelInitializer: No spawn point found for level {levelNumber}!");
        }
    }
}

using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class RestartButtonScript : MonoBehaviour
{
    private Button restartButton;

    void Awake()
    {
        restartButton = GetComponent<Button>();
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
            Debug.Log("RestartButtonScript: Listener added.");
        }
        else
        {
            Debug.LogError("RestartButtonScript requires a Button component on the same GameObject.");
        }
    }

    void OnDestroy()
    {
        
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(OnRestartClicked);
            Debug.Log("RestartButtonScript: Listener removed.");
        }
    }

    private void OnRestartClicked()
    {
        
        if (LevelManager.Instance != null)
        {
            Debug.Log("RestartButton clicked! Calling LevelManager.RestartCurrentLevel().");
            LevelManager.Instance.RestartCurrentLevel();
        }
        else
        {
            Debug.LogError("LevelManager.Instance is null. Cannot restart level.");
        }
    }
}
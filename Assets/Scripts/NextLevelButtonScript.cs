using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class NextLevelButtonScript : MonoBehaviour
{
    private Button nextLevelButton;

    void Awake()
    {
        nextLevelButton = GetComponent<Button>();
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);
            Debug.Log("NextLevelButtonScript: Listener added.");
        }
        else
        {
            Debug.LogError("NextLevelButtonScript requires a Button component on the same GameObject.");
        }
    }

    void OnDestroy()
    {
        
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveListener(OnNextLevelClicked);
            Debug.Log("NextLevelButtonScript: Listener removed.");
        }
    }

    private void OnNextLevelClicked()
    {
        
        if (LevelManager.Instance != null)
        {
            Debug.Log("NextLevelButton clicked! Calling LevelManager.LoadNextLevel().");
            LevelManager.Instance.LoadNextLevel();
        }
        else
        {
            Debug.LogError("LevelManager.Instance is null. Cannot load next level.");
        }
    }
}
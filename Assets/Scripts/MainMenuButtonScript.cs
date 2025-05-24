using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class MainMenuButtonScript : MonoBehaviour
{
    private Button mainMenuButton;

    void Awake()
    {
        mainMenuButton = GetComponent<Button>();
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            Debug.Log("MainMenuButtonScript: Listener added.");
        }
        else
        {
            Debug.LogError("MainMenuButtonScript requires a Button component on the same GameObject.");
        }
    }

    void OnDestroy()
    {
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
            Debug.Log("MainMenuButtonScript: Listener removed.");
        }
    }

    private void OnMainMenuClicked()
    {
        
        if (LevelManager.Instance != null)
        {
            Debug.Log("MainMenuButton clicked! Calling LevelManager.GoToMainMenu().");
            LevelManager.Instance.GoToMainMenu();
        }
        else
        {
            Debug.LogError("LevelManager.Instance is null. Cannot go to Main Menu.");
        }
    }
}
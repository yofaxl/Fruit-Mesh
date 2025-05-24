using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;    
    public GameObject levelSelectPanel; 

    [Header("UI Elements")]
    public Button playButton;           
    public Button quitButton;           

    private void Start()
    {
        
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);

        
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        
        
        LevelManager.Instance.CreateLevelButtons();
    }

    private void OnQuitButtonClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 
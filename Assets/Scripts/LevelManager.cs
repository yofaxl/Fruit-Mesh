using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    private int totalLevels = 6; 
    [Tooltip("The highest level reached. Only levels <= reachedLevel are unlocked.")]
    private int reachedLevel = 1; 

    [Header("UI References")]
    public GameObject levelSelectPanel;
    public Transform levelButtonContainer;
    public GameObject levelButtonPrefab;

    [Header("Sprites")]
    public Sprite[] unlockedSprites; 
    public Sprite[] lockedSprites;   

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        LoadLevelProgress();

        
        if (SceneManager.GetActiveScene().name == "MainMenu") 
        {
            levelSelectPanel = GameObject.Find("LevelSelectPanel"); 
            
            GameObject levelButtonContainerObject = GameObject.Find("LevelButtonContainer");
            if (levelButtonContainerObject != null) levelButtonContainer = levelButtonContainerObject.transform;

           

            if (levelSelectPanel != null)
                levelSelectPanel.SetActive(false);
        }
    }

    private void LoadLevelProgress()
    {
        
        reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);
        
        if (reachedLevel > totalLevels) reachedLevel = totalLevels;
    }

    private void SaveLevelProgress()
    {
       
        PlayerPrefs.SetInt("ReachedLevel", reachedLevel);
        PlayerPrefs.Save();
    }

    public void CreateLevelButtons()
    {
        foreach (Transform child in levelButtonContainer)
            Destroy(child.gameObject);

        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelButtonContainer);
            LevelButton levelButtonScript = buttonObj.GetComponent<LevelButton>();
            Button btn = buttonObj.GetComponent<Button>();

            Sprite unlockedSprite = (i - 1 < unlockedSprites.Length) ? unlockedSprites[i - 1] : null;
            Sprite lockedSprite = (i - 1 < lockedSprites.Length) ? lockedSprites[i - 1] : null;

            
            levelButtonScript.buttonImage = buttonObj.GetComponent<Image>();
            levelButtonScript.unlockedSprite = unlockedSprite;
            levelButtonScript.lockedSprite = lockedSprite;

            bool isUnlocked = i <= reachedLevel;
            bool shouldBeVisibleAndLocked = i == reachedLevel + 1 && i <= totalLevels;

            if (isUnlocked)
            {
                levelButtonScript.SetButton(true); 
                int levelNumber = i; 
                btn.onClick.AddListener(() => LoadLevel(levelNumber));
                btn.interactable = true;
                AddButtonSFXListeners(btn);
            }
            else if (shouldBeVisibleAndLocked || i > reachedLevel + 1)
            {
                levelButtonScript.SetButton(false);
                btn.interactable = false; 
            }
        }
    }

    public void LoadLevel(int levelNumber)
    {
        Debug.Log("Loading Level " + levelNumber);
        
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
       
   

    
        Debug.Log("Starting Coroutine to unload WinPanel and load Level " + levelNumber);
        StartCoroutine(UnloadWinPanelSceneAndLoadLevel("Level" + levelNumber));
    }

    public void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        CreateLevelButtons(); 
        
         StartCoroutine(UnloadWinPanelScene());
    }

    public void RestartLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.StartsWith("Level"))
        {
            int levelNumberToRestart = int.Parse(currentSceneName.Substring(5));
            LoadLevel(levelNumberToRestart);
        }
        else
        {
            Debug.LogError("RestartLevel called from a non-level scene!");
            ShowLevelSelect();
        }
    }

    public void LevelCompleted()
    {
       
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.StartsWith("Level"))
        {
            int completedLevelNumber = int.Parse(currentSceneName.Substring(5));

        
            if (completedLevelNumber >= reachedLevel && completedLevelNumber < totalLevels)
            {
                reachedLevel = completedLevelNumber + 1;
                SaveLevelProgress(); 
            }
            else if (completedLevelNumber == totalLevels)
            {
               
                Debug.Log("All levels completed!");
                
                 if (completedLevelNumber >= reachedLevel) {
                     reachedLevel = completedLevelNumber;
                     SaveLevelProgress();
                 }
            }

            
            SceneManager.LoadSceneAsync("WinUIPanel", LoadSceneMode.Additive);

            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayWinSound();
            }
            else
            {
                Debug.LogWarning("AudioManager instance is null when level completed.");
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        if (scene.name == "MainMenu")
        {
            Debug.Log("MainMenu scene loaded, re-acquiring UI references.");
          
            levelSelectPanel = GameObject.Find("LevelSelectPanel"); 
            
            GameObject levelButtonContainerObject = GameObject.Find("LevelButtonContainer"); 
            if (levelButtonContainerObject != null) levelButtonContainer = levelButtonContainerObject.transform;
            else Debug.LogError("LevelButtonContainer GameObject not found in MainMenu scene.");

         
            if (levelSelectPanel != null)
            {
                 levelSelectPanel.SetActive(false);
              
            }
             else Debug.LogError("LevelSelectPanel GameObject not found in MainMenu scene.");

            
             Button playButton = GameObject.Find("PlayButton")?.GetComponent<Button>(); 
             if (playButton != null)
             {
                 
                 playButton.onClick.AddListener(() => AudioManager.Instance?.PlayUIButtonSound());
             }
             else Debug.LogWarning("PlayButton not found in MainMenu scene.");

             Button quitButton = GameObject.Find("QuitButton")?.GetComponent<Button>();
             if (quitButton != null)
             {
                
                 quitButton.onClick.AddListener(() => AudioManager.Instance?.PlayUIButtonSound());
             }
             else Debug.LogWarning("QuitButton not found in MainMenu scene.");

        }
       
        else if (scene.name == "DeathUIPanel") 
        {
            Debug.Log("DeathUIPanel scene loaded, acquiring UI references and adding button listeners.");

          
            GameObject losePanel = GameObject.Find("LosePanel");

            if (losePanel != null)
            {
                
                Transform losePanelTransform = losePanel.transform;
                Button loseRetryButton = losePanelTransform.Find("RetryButton")?.GetComponent<Button>();
                Button loseGoToMainMenuButton = losePanelTransform.Find("MainMenuButton")?.GetComponent<Button>();

               
                if (loseRetryButton != null)
                {
                    loseRetryButton.onClick.RemoveAllListeners(); 
                    loseRetryButton.onClick.AddListener(RetryLevelFromDeathPanel);
                    
                    loseRetryButton.onClick.AddListener(() => AudioManager.Instance?.PlayUIButtonSound());
                }
                else Debug.LogError("RetryButton not found under LosePanel or does not have a Button component.");

                if (loseGoToMainMenuButton != null)
                {
                    loseGoToMainMenuButton.onClick.RemoveAllListeners(); 
                    loseGoToMainMenuButton.onClick.AddListener(GoToMainMenuFromDeathPanel);
                    
                    loseGoToMainMenuButton.onClick.AddListener(() => AudioManager.Instance?.PlayUIButtonSound());
                }
                else Debug.LogError("MainMenuButton not found under LosePanel or does not have a Button component.");
            }
            else Debug.LogError("LosePanel GameObject not found in DeathUIPanel scene.");
        }
         
        
        else if (scene.name == "WinUIPanel") 
        {
             Debug.Log("WinUIPanel scene loaded, acquiring UI references and adding button listeners.");

             
             GameObject winPanel = GameObject.Find("WinPanel"); 

             if (winPanel != null)
             {
                
                 Transform winPanelTransform = winPanel.transform;
                 Button winRestartButton = winPanelTransform.Find("RestartButton")?.GetComponent<Button>(); 
                 Button winMainMenuButton = winPanelTransform.Find("MainMenuButton")?.GetComponent<Button>(); 
                 Button winNextLevelButton = winPanelTransform.Find("NextLevelButton")?.GetComponent<Button>(); 

                 if (winRestartButton != null)
                 {
                     winRestartButton.onClick.RemoveAllListeners();
                     winRestartButton.onClick.AddListener(RestartCurrentLevel);
       
                     winRestartButton.onClick.AddListener(() => AudioManager.Instance?.PlayUIButtonSound());
                 }
                 else Debug.LogError("RestartButton not found under WinPanel or does not have a Button component.");

                 if (winMainMenuButton != null)
                 {
                     winMainMenuButton.onClick.RemoveAllListeners(); 
                     winMainMenuButton.onClick.AddListener(GoToMainMenu);
                    
                     winMainMenuButton.onClick.AddListener(() => AudioManager.Instance?.PlayUIButtonSound());
                 }
                 else Debug.LogError("MainMenuButton not found under WinPanel or does not have a Button component.");

                  if (winNextLevelButton != null)
                 {
                     winNextLevelButton.onClick.RemoveAllListeners(); 
                     winNextLevelButton.onClick.AddListener(LoadNextLevel);
                      
                     winNextLevelButton.onClick.AddListener(() => AudioManager.Instance?.PlayUIButtonSound());
                 }
                 else Debug.LogError("NextLevelButton not found under WinPanel or does not have a Button component.");
             }
             else Debug.LogError("WinPanel GameObject not found in WinUIPanel scene.");

        }

    }

    private void SetupWinPanelButtons(Scene winPanelScene)
    {
        
    }

    public void RestartCurrentLevel()
    {
        
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.StartsWith("Level"))
        {
            int levelNumberToRestart = int.Parse(currentSceneName.Substring(5));
            LoadLevel(levelNumberToRestart);
        }
        else
        {
            Debug.LogError("RestartLevel called from a non-level scene!");
            ShowLevelSelect(); 
        }
    }

    public void GoToMainMenu()
    {
        
        StartCoroutine(UnloadWinPanelSceneAndLoadLevel("MainMenu")); 
    }

    public void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.StartsWith("Level"))
        {
            int currentLevelNumber = int.Parse(currentSceneName.Substring(5));
            int nextLevelNumber = currentLevelNumber + 1;

           
            if (nextLevelNumber <= totalLevels)
            {
                
                StartCoroutine(UnloadWinPanelSceneAndLoadLevel("Level" + nextLevelNumber));
            }
            else
            {
               
                 GoToMainMenu();
            }
        }
    }

    
    private IEnumerator UnloadWinPanelScene()
    {
         Scene winPanelScene = SceneManager.GetSceneByName("WinUIPanel");
        if (winPanelScene.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync("WinUIPanel");
        }
    }

    
    private IEnumerator UnloadWinPanelSceneAndLoadLevel(string levelSceneName)
    {
        Debug.Log("Coroutine: Unloading WinPanel scene...");
        yield return UnloadWinPanelScene();
        Debug.Log("Coroutine: WinPanel scene unloaded. Loading " + levelSceneName + "...");
        SceneManager.LoadScene(levelSceneName);
         Debug.Log("Coroutine: " + levelSceneName + " loaded.");
    }

     private void OnEnable()
     {
         
         SceneManager.sceneLoaded += OnSceneLoaded;
     }

     private void OnDisable()
     {
         
         SceneManager.sceneLoaded -= OnSceneLoaded;
     }


public void ShowLosePanel()
{
    Debug.Log("Showing Lose Panel.");
   
    SceneManager.LoadSceneAsync("DeathUIPanel", LoadSceneMode.Additive); 
}


public void RetryLevelFromDeathPanel()
{
    Debug.Log("Retry button clicked on Lose Panel.");
   
    string currentSceneName = SceneManager.GetActiveScene().name;
    if (currentSceneName.StartsWith("Level"))
    {
       
        StartCoroutine(UnloadDeathPanelSceneAndLoadLevel(currentSceneName));
    }
    else
    {
        
        Debug.LogError("RetryLevelFromDeathPanel called from a non-level scene!");
        GoToMainMenuFromDeathPanel();
    }
}


public void GoToMainMenuFromDeathPanel()
{
    Debug.Log("Main Menu button clicked on Lose Panel.");
    
    StartCoroutine(UnloadDeathPanelSceneAndLoadLevel("MainMenu")); 
}


private IEnumerator UnloadDeathPanelScene()
{
     Scene deathPanelScene = SceneManager.GetSceneByName("DeathUIPanel"); 
    if (deathPanelScene.isLoaded)
    {
        yield return SceneManager.UnloadSceneAsync("DeathUIPanel"); 
    }
}

 
private IEnumerator UnloadDeathPanelSceneAndLoadLevel(string sceneName)
{
    Debug.Log("Coroutine: Unloading DeathPanel scene...");
    yield return UnloadDeathPanelScene();
    Debug.Log("Coroutine: DeathPanel scene unloaded. Loading " + sceneName + "...");
    SceneManager.LoadScene(sceneName);
     Debug.Log("Coroutine: " + sceneName + " loaded.");
}


public void ResetLevelProgress()
{
    Debug.Log("Resetting level progress.");
   
    PlayerPrefs.DeleteKey("ReachedLevel");
   
    PlayerPrefs.Save(); 

    
    reachedLevel = 1;

    
    SceneManager.LoadScene("MainMenu"); 
}

private void AddButtonSFXListeners(Button btn)
{
    btn.onClick.AddListener(() => AudioManager.Instance?.PlayUIButtonSound());
}

}


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    public int totalLevels = 8; // Her zaman 8 olacak
    [Tooltip("The highest level reached. Only levels <= reachedLevel are unlocked.")]
    public int reachedLevel = 1; // Inspector'dan değiştir

    [Header("UI References")]
    public GameObject levelSelectPanel;
    public Transform levelButtonContainer;
    public GameObject levelButtonPrefab;

    [Header("Sprites")]
    public Sprite[] unlockedSprites; // Inspector'dan sırayla atayacaksın (1N, 2N, 3N, ...)
    public Sprite[] lockedSprites;   // Inspector'dan sırayla atayacaksın (1K, 2K, 3K, ...) // DİKKAT: Kilitli sprite'lar artık 1. seviyeden itibaren olmalı

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Kayıtlı level verilerini yükle
        LoadLevelProgress();
        // Level seçim panelini başlangıçta gizle
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false);
    }

    private void LoadLevelProgress()
    {
        // PlayerPrefs'ten kayıtlı verileri yükle
        reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 8);
    }

    private void SaveLevelProgress()
    {
        // Verileri kaydet
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

            // Sprite atamaları
            Sprite unlockedSprite = (i - 1 < unlockedSprites.Length) ? unlockedSprites[i - 1] : null;
            Sprite lockedSprite = (i - 1 < lockedSprites.Length) ? lockedSprites[i - 1] : null;

            // LevelButton script'ine referansları ve sprite'ları ata
            levelButtonScript.buttonImage = buttonObj.GetComponent<Image>();
            levelButtonScript.unlockedSprite = unlockedSprite;
            levelButtonScript.lockedSprite = lockedSprite;

            bool isUnlocked = i <= reachedLevel;
            bool shouldBeVisibleAndLocked = i == reachedLevel + 1 && i <= totalLevels;

            if (isUnlocked)
            {
                levelButtonScript.SetButton(true); // Açık
                int levelNumber = i; // Yerel değişken oluştur
                btn.onClick.AddListener(() => LoadLevel(levelNumber));
                btn.interactable = true;
            }
            else if (shouldBeVisibleAndLocked || i > reachedLevel + 1)
            {
                levelButtonScript.SetButton(false); // Kilitli
                btn.interactable = false; // Kilitli butonlara tıklanamaz
            }
        }
    }

    public void LoadLevel(int levelNumber)
    {
        Debug.Log("Loading Level " + levelNumber);
        levelSelectPanel.SetActive(false);
        SceneManager.LoadScene("Level" + levelNumber);
    }

    public void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        CreateLevelButtons(); // Butonları güncelle
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
}
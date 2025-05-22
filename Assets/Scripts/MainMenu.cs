using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;    // Ana menü paneli
    public GameObject levelSelectPanel; // Level seçim paneli

    [Header("UI Elements")]
    public Button playButton;           // Play butonu
    public Button quitButton;           // Çıkış butonu

    private void Start()
    {
        // Başlangıçta sadece ana menüyü göster
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);

        // Butonlara tıklama olaylarını ekle
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        // Ana menüyü kapat, level seçim ekranını aç
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        
        // LevelManager'ı bul ve level butonlarını oluştur
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
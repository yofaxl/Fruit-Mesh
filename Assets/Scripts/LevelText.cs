using UnityEngine;
using TMPro;

public class LevelText : MonoBehaviour
{
    private TextMeshProUGUI levelText;
    private Canvas canvas;

    void Start()
    {
        levelText = GetComponent<TextMeshProUGUI>();
        canvas = GetComponentInParent<Canvas>();
        
        if (canvas != null)
        {
            // Ensure the canvas is in Screen Space - Overlay mode
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
        }

        if (levelText != null)
        {
            // Ensure the text is visible
            levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, 1f);
        }
    }
} 
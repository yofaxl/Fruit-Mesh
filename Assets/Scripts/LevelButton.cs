using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Image buttonImage;
    public Sprite unlockedSprite;
    public Sprite lockedSprite;

    // Level açık mı, kilitli mi ona göre sprite değiştir
    public void SetButton(bool isUnlocked)
    {
        if (isUnlocked)
        {
            buttonImage.sprite = unlockedSprite;
        }
        else
        {
            buttonImage.sprite = lockedSprite;
        }
    }
} 
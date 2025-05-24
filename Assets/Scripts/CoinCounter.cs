using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;

    public TMP_Text coinText;
    public int currentCoins = 0;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        coinText.text = "FRUITS: " + currentCoins.ToString();
    }

    public void IncreaseCoins(int v)
    {
        currentCoins += v/5;
        coinText.text = "FRUITS: " + currentCoins.ToString();
    }


}

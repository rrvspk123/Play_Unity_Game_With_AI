using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TextMesh livesText;
    public TextMesh coinsText;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        livesText.text = "Lives: " + gameManager.GetLives().ToString();
        coinsText.text = "Coins: " + gameManager.GetCoins().ToString();
        
        livesText.GetComponent<Renderer>().sortingLayerName = "UI";
        livesText.GetComponent<Renderer>().sortingOrder = 1;
        coinsText.GetComponent<Renderer>().sortingLayerName = "UI";
        coinsText.GetComponent<Renderer>().sortingOrder = 1;
    }
}

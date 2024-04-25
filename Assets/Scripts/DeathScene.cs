using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScene : MonoBehaviour
{
    private GameManager gameManager;
    public TextMesh scoreText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        int currentScore = gameManager.score;
        scoreText.text = "Score: " + currentScore.ToString();
    }
}
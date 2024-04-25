using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }
    public int score { get; private set; }

    private string userID;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        userID = SystemInfo.deviceUniqueIdentifier;
    }

    public void NewGame()
    {
        if (PlayerFile.signIn)
        {
            lives = 3;
            coins = 0;
            score = 0;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

            LoadLevel(1, 1);
        } else {
            Debug.Log("Failed to SignIn");
        }
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NewLevel()
    {
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;

        if (lives > 0)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    private IEnumerator GameOverCoroutine()
    {
        SceneManager.LoadScene("DeathScene");
        PlayerFile.UpdateScore();
        yield return new WaitForSeconds(10f);
        PlayerFile.signIn = true;
        NewGame();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    public void AddCoin()
    {
        coins++;
        score += 10;

        if (coins == 100)
        {
            AddLife();
            coins = 0;
        }
    }

    public void AddLife()
    {
        lives++;
        score += 15;
    }

    public void AddScore()
    {
        score += 50;
    }

    public int GetLives()
    {
        return lives;
    }

    public int GetCoins()
    {
        return coins;
    }

    public int GetScore()
    {
        return score;
    }




}
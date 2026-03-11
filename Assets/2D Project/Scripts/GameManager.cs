using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreMenu;
    [SerializeField] private float endSceneDelay = 1.5f;

    private int currentScore = 0;
    private int highScore = 0;
    private bool gameEnded;

    void Start()
    {
        Time.timeScale = 1f;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        Enemy.OnEnemyDied += OnEnemyDied;
        UpdateScoreDisplay();
    }

    void Update()
    {
        if (gameEnded)
        {
            return;
        }
    }

    void OnEnemyDied(float score)
    {
        int pointsInt = (int)score;
        currentScore += pointsInt;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateScoreDisplay();
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDied -= OnEnemyDied;
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score \n {currentScore.ToString("D4")}";
        }
        if (highScoreText != null)
        {
            highScoreText.text = $"HI - Score \n {highScore.ToString("D4")}";
        }
    }

    public void HandlePlayerDied()
    {
        EndGame();
    }

    public void HandleAllEnemiesCleared()
    {
        EndGame();
    }

    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
        UpdateScoreDisplay();
    }

    private void EndGame()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        StartCoroutine(LoadCreditsAfterDelay());
    }

    private System.Collections.IEnumerator LoadCreditsAfterDelay()
    {
        yield return new WaitForSecondsRealtime(endSceneDelay);

        SceneLoader sceneLoader = FindFirstObjectByType<SceneLoader>();
        if (sceneLoader != null)
        {
            sceneLoader.LoadCredits();
        }
        else
        {
            Debug.LogWarning("SceneLoader not found. Could not load Credits scene.");
        }
    }
}


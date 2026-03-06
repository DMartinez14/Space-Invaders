using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreMenu;
    private int currentScore = 0;
    private int highScore = 0;
    private Coroutine hideScoreMenuCoroutine;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        Enemy.OnEnemyDied += OnEnemyDied;
        UpdateScoreDisplay();
        // Start coroutine to hide scoreMenu after 3 seconds
        if (scoreMenu != null && scoreMenu.gameObject.activeSelf)
        {
            hideScoreMenuCoroutine = StartCoroutine(HideTMP(scoreMenu, 3f));
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (scoreMenu != null && scoreMenu.gameObject.activeSelf)
            {
                // Stop the coroutine if it's running
                if (hideScoreMenuCoroutine != null)
                {
                    StopCoroutine(hideScoreMenuCoroutine);
                    hideScoreMenuCoroutine = null;
                }
                scoreMenu.gameObject.SetActive(false);
            }
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

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore.ToString("D4")}";
        }
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScore.ToString("D4")}";
        }
    }

    private System.Collections.IEnumerator HideTMP(TextMeshProUGUI tmpObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (tmpObject != null && tmpObject.gameObject.activeSelf)
        {
            tmpObject.gameObject.SetActive(false);
        }
    }
}


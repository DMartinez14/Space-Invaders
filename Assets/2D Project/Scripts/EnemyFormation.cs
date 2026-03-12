using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    public float stepInterval = 0.5f; 
    public float stepDistance = 0.5f; 
    public float stepDown = 0.5f;

    [Header("Boundaries")]
    public float leftLimit = -8f;
    public float rightLimit = 8f;

    private float direction = 1f; 
    private float stepTimer = 0f;

    void Update()
    {
        // Count down the timer
        stepTimer -= Time.deltaTime;

        // When timer reaches zero, take a step
        if (stepTimer <= 0f)
        {
            stepTimer = stepInterval;

            // Move the whole formation horizontally
            transform.position += Vector3.right * direction * stepDistance;

            // When hitting a boundary, flip direction and step down
            if (transform.position.x >= rightLimit)
            {
                Step(-1f);
            }
            else if (transform.position.x <= leftLimit)
            {
                Step(1f);
            }
        }
    }

    private void Step(float newDirection)
    {
        direction = newDirection;
        transform.position += Vector3.down * stepDown;
    }

    // Called by Enemy when it gets destroyed
    public void OnEnemyKilled()
    {
        int remaining = transform.childCount - 1; 
        if (remaining > 0)
        {
            // Speed up as fewer enemies remain (reduce time between steps)
            stepInterval = Mathf.Max(0.1f, stepInterval - 0.02f);
        }
        else
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.HandleAllEnemiesCleared();
            }
        }
    }
}

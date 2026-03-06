using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    public float speed = 2f;
    public float stepDown = 0.5f;

    [Header("Boundaries")]
    public float leftLimit = -8f;
    public float rightLimit = 8f;

    private float direction = 1f; // 1 = right, -1 = left

    void Update()
    {
        // Move the whole formation horizontally
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

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

    private void Step(float newDirection)
    {
        direction = newDirection;
        transform.position += Vector3.down * stepDown;
    }

    // Called by Enemy when it gets destroyed
    public void OnEnemyKilled()
    {
        int remaining = transform.childCount - 1; // -1 because child isn't destroyed yet
        if (remaining > 0)
        {
            // Speed up as fewer enemies remain (caps at 3x original speed)
            speed += 0.1f;
        }
    }
}

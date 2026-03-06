using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float speed = 5;
    public Vector2 direction = Vector2.up;
    public bool isEnemyBullet;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HandlePlayerHit(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandlePlayerHit(collision.gameObject);
    }

    private void HandlePlayerHit(GameObject other)
    {
        GameObject playerObject = other.CompareTag("Player") ? other : other.transform.root.gameObject;

        if (isEnemyBullet && playerObject.CompareTag("Player"))
        {
            Debug.Log("GAME OVER");
            Destroy(playerObject);
            Destroy(gameObject);
            Time.timeScale = 0f;
        }
    }
}

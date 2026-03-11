using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioClip ticClip;
    public AudioClip tacClip;
    public GameObject bulletPrefab;
    public Transform shootOffsetTransform;
    public float shootInterval = 2f;
    private float shootTimer;
    public delegate void EnemyDiedFunc(float points);
    public static event EnemyDiedFunc OnEnemyDied;

    void Start()
    {
        // randomize initial shot
        shootTimer = Random.Range(0f, shootInterval); 
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            // add some randomness
            shootTimer = shootInterval + Random.Range(-0.5f, 0.5f); 
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && shootOffsetTransform != null)
        {
            GameObject shot = Instantiate(bulletPrefab, shootOffsetTransform.position, Quaternion.identity);
            Bullet bullet = shot.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.direction = Vector2.down;
                bullet.isEnemyBullet = true;
            }

            Collider2D bulletCollider = shot.GetComponent<Collider2D>();
            if (bulletCollider != null)
            {
                Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                for (int i = 0; i < allEnemies.Length; i++)
                {
                    Collider2D[] enemyColliders = allEnemies[i].GetComponentsInChildren<Collider2D>();
                    for (int j = 0; j < enemyColliders.Length; j++)
                    {
                        Physics2D.IgnoreCollision(enemyColliders[j], bulletCollider);
                    }
                }
            }

            Destroy(shot, 3f);
            // Optionally trigger shoot animation here
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ouch!");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            // Ignore collisions with Cover
            if (collision.gameObject.CompareTag("Cover"))
            {
                Debug.Log("Enemy collided with Cover - no effect.");
                return;
            }
            if (bullet != null && !bullet.isEnemyBullet)
            {
                Destroy(collision.gameObject);
                EnemyFormation formation = GetComponentInParent<EnemyFormation>();
                if (formation != null)
                {
                    formation.OnEnemyKilled();
                }

                Destroy(gameObject);
                int points = 0;
                switch (gameObject.tag)
                {
                    case "Enemy":
                        points = 10;
                        break;
                    case "20points":
                        points = 20;
                        break;
                    case "30points":
                        points = 30;
                        break;
                    case "50points":
                        points = 50;
                        break;
                }
                Debug.Log($"Enemy tag: {gameObject.tag}, Points awarded: {points}");
                OnEnemyDied?.Invoke(points);
            }
        // todo - trigger death animation
    }
    public void PlayTicSound()
    {
        GetComponent<AudioSource>().PlayOneShot(ticClip);
    }
    public void PlayTacSound()
    {
        GetComponent<AudioSource>().PlayOneShot(tacClip);
    }
}

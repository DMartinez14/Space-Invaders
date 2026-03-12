using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootOffsetTransform;
    
    [Header("Audio")]
    public AudioClip shootClip;
    public AudioClip deathClip;
    [Range(0f, 1f)] public float shootVolume = 0.35f;
    [Range(0f, 1f)] public float deathVolume = 0.7f;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GameObject shot = Instantiate(bulletPrefab, shootOffsetTransform.position, Quaternion.identity);
            Bullet bullet = shot.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.direction = Vector2.up;
                bullet.isEnemyBullet = false;
            }

            Debug.Log("Bang!");

            // Destroy the bullet after 3 seconds
            Destroy(shot, 3f);
            // Trigger shoot animation
            if (animator != null)
            {
                animator.SetTrigger("Shot Trigger");
            }
        }
        else if (Keyboard.current != null && Keyboard.current.leftArrowKey.isPressed)
        {
            transform.position += Vector3.left * Time.deltaTime * 5f;
        }
        else if (Keyboard.current != null && Keyboard.current.rightArrowKey.isPressed)
        {
            transform.position += Vector3.right * Time.deltaTime * 5f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HandleEnemyBulletHit(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleEnemyBulletHit(collision.gameObject);
    }

    private void HandleEnemyBulletHit(GameObject other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null && bullet.isEnemyBullet)
        {
            Debug.Log("GAME OVER");

            // Play death sound directly from script.
            if (deathClip != null)
            {
                AudioSource.PlayClipAtPoint(deathClip, transform.position, deathVolume);
            }

            // Notify GameManager
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.HandlePlayerDied();
            }

            // Destroy bullet immediately
            Destroy(other);

            // No death animation flow, destroy player immediately.
            Destroy(gameObject);
        }
    }

    // Called from an Animation Event in the shoot clip.
    public void PlayShootSound()
    {
        if (audioSource != null && shootClip != null)
        {
            audioSource.PlayOneShot(shootClip, shootVolume);
        }
    }

}

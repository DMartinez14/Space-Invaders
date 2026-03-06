using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootOffsetTransform;

    void Start()
    {
        // todo - get and cache animator
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

            // todo - destroy the bullet after 3 seconds
            Destroy(shot, 3f);
            // todo - trigger shoot animation
            GetComponent<Animator>().SetTrigger("Shot Trigger");
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
            Destroy(other);
            Destroy(gameObject);
            Time.timeScale = 0f;
        }
    }
}

using UnityEngine;

public class Cover : MonoBehaviour
{
    public int health = 6;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            health--;
            Destroy(collision.gameObject);
            Debug.Log($"Cover hit! Health remaining: {health}");
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class MinionBullet : MonoBehaviour
{
    public float speed = 5f; // Tốc độ đạn
    private Vector2 direction;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        rb.velocity = direction * speed; // Di chuyển đạn
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerShipTag"))
        {
            Debug.Log("🔥 Đạn Minion trúng Player! Gây 1 sát thương.");
            col.GetComponent<PlayerController>()?.TakeDamage(1);
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Tự hủy khi ra khỏi màn hình
    }
}

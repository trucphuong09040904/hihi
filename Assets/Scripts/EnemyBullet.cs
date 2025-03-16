using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed = 5f; // Tốc độ đạn
    Vector2 _direction; // Hướng di chuyển của đạn
    bool isReady = false; // Xác định đã có hướng di chuyển chưa

    void Start() { }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
        isReady = true;
    }

    void Update()
    {
        if (isReady)
        {
            transform.position += (Vector3)(_direction * speed * Time.deltaTime);

            // Kiểm tra nếu đạn ra khỏi màn hình thì hủy
            if (!GetComponent<Renderer>().isVisible)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerShipTag")) // Kiểm tra va chạm với Player
        {
            Destroy(gameObject); // Hủy viên đạn
        }
    }
}

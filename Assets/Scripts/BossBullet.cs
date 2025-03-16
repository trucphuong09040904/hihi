using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Vector2 direction; // Hướng di chuyển của đạn
    private float speed = 5f;
    private int damage;

    public void SetTarget(Transform playerTarget, int bulletDamage)
    {
        if (playerTarget != null)
        {
            // Tính toán hướng từ Boss đến Player lúc bắn
            direction = (playerTarget.position - transform.position).normalized;
        }
        damage = bulletDamage;
    }

    void Update()
    {
        // Đạn di chuyển theo hướng đã tính sẵn
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerShipTag"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

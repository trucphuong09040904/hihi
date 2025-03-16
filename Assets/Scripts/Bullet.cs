using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; // Tốc độ bay của đạn
    public float lifeTime = 5f; // Thời gian tự hủy nếu không va chạm
    public int damage = 100; // Sát thương gây ra
    private Vector2 moveDirection;
    private string shooterTag; // Lưu tag của người bắn

    void Start()
    {
        Destroy(gameObject, lifeTime); // Hủy đạn sau một thời gian
    }

    public void SetDirection(Vector2 direction, string shooter)
    {
        moveDirection = direction.normalized;
        shooterTag = shooter; // Gán tag của người bắn
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu đạn Boss bắn trúng Player
        if (shooterTag == "Boss" && collision.CompareTag("PlayerShipTag"))
        {
            Debug.Log("🔥 Player bị trúng đạn Boss! Mất 2 HP.");
            collision.GetComponent<PlayerController>()?.TakeDamage(2);
            Destroy(gameObject);
        }
        // Kiểm tra nếu đạn Player bắn trúng Boss
        else if (shooterTag == "PlayerBulletTag" && collision.CompareTag("Boss"))
        {
            Debug.Log("💥 Boss bị bắn trúng! Mất 100 HP.");
            BossAI boss = collision.GetComponent<BossAI>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                GameScore.instance.AddScore(100);
            }
            Destroy(gameObject);
        }
    }
}

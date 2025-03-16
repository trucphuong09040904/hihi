using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject ExplosionGO; // Prefab vụ nổ
    public float speed;

    private GameScore gameScore; // Tham chiếu đến GameScore
    private bool isDestroyed = false; // Biến ngăn chặn cộng điểm hai lần

    void Start()
    {
        // Tìm đối tượng chứa GameScore
        gameScore = FindObjectOfType<GameScore>();

        if (gameScore == null)
        {
            Debug.LogError("EnemyController: Không tìm thấy GameScore! Hãy đảm bảo có GameScore trong scene.");
        }
    }

    void Update()
    {
        // Di chuyển enemy xuống dưới
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Xóa enemy nếu đi quá màn hình
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Ngăn chặn cộng điểm nhiều lần
        if (isDestroyed) return;

        // Kiểm tra va chạm với đạn hoặc tàu người chơi
        if (col.CompareTag("PlayerShipTag") || col.CompareTag("PlayerBulletTag"))
        {
            isDestroyed = true; // Đánh dấu enemy đã bị phá hủy

            PlayExplosion();

            if (gameScore != null)
            {
                gameScore.Score += 100;
                Debug.Log("Enemy bị bắn! Điểm hiện tại: " + gameScore.Score);
            }

            // Xóa đạn ngay sau khi va chạm để tránh cộng điểm nhiều lần
            if (col.CompareTag("PlayerBulletTag"))
            {
                Destroy(col.gameObject);
            }

            Destroy(gameObject);
        }
    }

    void PlayExplosion()
    {
        Instantiate(ExplosionGO, transform.position, Quaternion.identity);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float moveRange = 5f;
    public float shootInterval = 2f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform player;

    public int maxHealth = 5000;
    private int currentHealth;
    public Slider healthBar;
    public RectTransform healthBarUI;
    public GameObject explosionEffect;

    private Vector3 targetPosition;
    private Vector2 minScreenBounds, maxScreenBounds;
    private bool isEnraged = false; // Trạng thái cuồng nộ
    private bool isDying = false; // Cờ trạng thái boss đang chết
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        minScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(MoveRandomly());
        StartCoroutine(ShootAtPlayer());
    }

    void Update()
    {
        if (healthBarUI != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
            healthBarUI.position = screenPos;
        }
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            float minY = minScreenBounds.y + (maxScreenBounds.y - minScreenBounds.y) * 0.25f; // Giới hạn di chuyển 3/4 màn hình trên
            float maxY = maxScreenBounds.y;

            targetPosition = new Vector3(
                Random.Range(minScreenBounds.x, maxScreenBounds.x),
                Random.Range(minY, maxY), // Giới hạn di chuyển trên 3/4 màn hình
                transform.position.z
            );

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
    }


    IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);

            if (player != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.tag = "Bullet";
                Vector2 direction = (player.position - firePoint.position).normalized;
                bullet.GetComponent<Bullet>().SetDirection(direction, "Boss");
            }
        }
    }

    public void TakeDamage(int damage, Vector3 hitPosition)
    {
        if (isDying) return; // Nếu đang chết thì không nhận thêm damage

        currentHealth -= damage;
        healthBar.value = currentHealth;

        GameScore.instance.AddScore(100);

        if (currentHealth <= 2000 && !isEnraged)
        {
            EnterRageMode();
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(ExplodeBoss(hitPosition));
        }
    }


    void EnterRageMode()
    {
        isEnraged = true;
        moveSpeed *= 3;
        Debug.Log("🔥 Boss đã vào trạng thái CUỒNG NỘ!");

        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.Shake(0.5f, 0.1f));
        }

        StartCoroutine(FlashRedEffect());
    }

    IEnumerator FlashRedEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Hiệu ứng nổ từ vị trí va chạm
    IEnumerator ExplodeBoss(Vector3 hitPosition)
    {
        Debug.Log("💥 Boss sắp nổ tung!");
        isDying = true; // Đánh dấu boss đã bắt đầu chết

        // Dừng di chuyển & bắn bằng cách không cho phép boss thực hiện các hành động khác
        moveSpeed = 0;
        
        // Hiệu ứng nhấp nháy đỏ khi chết
        StartCoroutine(FlashRedEffect());

        // Tạo vụ nổ ban đầu từ vị trí viên đạn va chạm
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, hitPosition, Quaternion.identity);
        }

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 randomExplosionPosition = new Vector3(
                transform.position.x + Random.Range(-1.5f, 1.5f),
                transform.position.y + Random.Range(-1.5f, 1.5f),
                transform.position.z
            );

            Instantiate(explosionEffect, randomExplosionPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.2f); // Giảm thời gian giữa các vụ nổ
            elapsedTime += 0.2f;
        }

        // Vụ nổ cuối cùng, lớn hơn
        if (explosionEffect != null)
        {
            GameObject bigExplosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            bigExplosion.transform.localScale = new Vector3(3f, 3f, 3f);
        }

        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
        Destroy(healthBarUI.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBulletTag"))
        {
            Debug.Log("💥 Boss bị bắn trúng! Mất 100 HP.");
            TakeDamage(100, collision.transform.position); // Truyền vị trí viên đạn
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("PlayerShipTag"))
        {
            Debug.Log("🔥 Boss va chạm với Player! Gây 2 sát thương.");
            collision.GetComponent<PlayerController>()?.TakeDamage(2);
        }
    }

}
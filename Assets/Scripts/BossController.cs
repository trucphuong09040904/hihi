using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Slider healthBar;
    public GameObject minionPrefab;
    public Transform[] minionSpawnPoints;
    public GameObject explosionEffect;

    public float moveSpeed = 3f;
    public float fireRate = 2f;
    private float nextFireTime;
    private Transform player;

    public int maxHealth = 3000;
    private int currentHealth;
    private bool spawnedMinions = false;
    private bool isEnraged = false;
    private Vector3 targetPosition;
    private Vector2 minScreenBounds, maxScreenBounds;
    private SpriteRenderer spriteRenderer;
    public int scorePerHit = 100;
    private bool isTakingDamage = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerShipTag").transform;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        minScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(MoveRandomly());
        StartCoroutine(FireAtPlayer());
    }

    void Update()
    {
        if (healthBar != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
            healthBar.transform.position = screenPos;
        }
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            float minY = minScreenBounds.y + (maxScreenBounds.y - minScreenBounds.y) * 0.25f;
            float maxY = maxScreenBounds.y - 1.5f;

            targetPosition = new Vector3(
                Random.Range(minScreenBounds.x + 1f, maxScreenBounds.x - 1f),
                Random.Range(minY, maxY),
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

    IEnumerator FireAtPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            if (player != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<BossBullet>().SetTarget(player, 2);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0 || isTakingDamage) return;

        isTakingDamage = true;
        currentHealth -= damage;
        healthBar.value = Mathf.Max(0, currentHealth);
        Debug.Log("Boss HP: " + currentHealth);

        if (GameScore.instance != null)
        {
            Debug.Log("🏆 Cộng điểm cho Player: " + scorePerHit);
            GameScore.instance.AddScore(scorePerHit);
        }

        if (currentHealth <= 1500 && !spawnedMinions)
        {
            SpawnMinions();
            spawnedMinions = true;
        }

        if (currentHealth <= 1000 && !isEnraged)
        {
            EnterRageMode();
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        isTakingDamage = false;
    }

    void EnterRageMode()
    {
        isEnraged = true;
        moveSpeed *= 2;
        Debug.Log("🔥 Boss vào trạng thái CUỒNG NỘ!");

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

    void SpawnMinions()
    {
        foreach (Transform spawnPoint in minionSpawnPoints)
        {
            GameObject minion = Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
            minion.tag = "Minion";
        }
    }

    void Die()
    {
        Debug.Log("💀 Boss đã chết, hủy hoàn toàn!");

        StopAllCoroutines();
        GetComponent<Collider2D>().enabled = false;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(healthBar.gameObject);

        foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Minion"))
        {
            Destroy(minion);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBulletTag"))
        {
            Debug.Log("💥 Boss bị bắn trúng! Mất 100 HP.");
            TakeDamage(100);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("PlayerShipTag"))
        {
            Debug.Log("🔥 Boss va chạm với Player! Gây 2 sát thương.");
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(2);
            }
        }
    }
}
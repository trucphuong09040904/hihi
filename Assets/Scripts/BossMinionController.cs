using System.Collections;
using UnityEngine;

public class BossMinionController : MonoBehaviour
{
    public float moveSpeed = 1.5f; // Giảm tốc độ so với Boss
    public float moveRange = 3f;
    public float shootInterval = 3f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Transform player;

    private Vector3 targetPosition;
    private Vector2 minScreenBounds, maxScreenBounds;
    private bool isAlive = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerShipTag").transform;

        minScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        StartCoroutine(MoveRandomly());
        StartCoroutine(ShootAtPlayer());
    }

    IEnumerator MoveRandomly()
    {
        while (isAlive)
        {
            float minY = minScreenBounds.y + (maxScreenBounds.y - minScreenBounds.y) * 0.3f;
            float maxY = maxScreenBounds.y - 1.8f;

            targetPosition = new Vector3(
                Mathf.Clamp(Random.Range(minScreenBounds.x + 1f, maxScreenBounds.x - 1f), minScreenBounds.x, maxScreenBounds.x),
                Mathf.Clamp(Random.Range(minY, maxY), minY, maxY),
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
        while (isAlive)
        {
            yield return new WaitForSeconds(shootInterval);

            if (player != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.tag = "EnemyBullet"; // Đạn không gây sát thương
                Vector2 direction = (player.position - firePoint.position).normalized;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * 5f; // Điều chỉnh tốc độ của đạn
                }
            }
        }
    }

    public void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }
}

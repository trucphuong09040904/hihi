using System.Collections;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public GameObject bulletPrefab; // Đạn của Minion
    public float bulletSpeed = 5f;
    public float fireRate = 1.5f; // Tốc độ bắn đạn
    public float lifetime = 8f; // Thời gian tồn tại trước khi bay ra khỏi màn hình

    private Vector3 targetPosition; // Vị trí sẽ đến khi bay vào
    private bool isEntering = true; // Cờ trạng thái vào màn hình
    private float enterSpeed = 5f; // Tốc độ bay vào
    private float exitSpeed = 5f; // Tốc độ bay ra
    private Vector3 exitDirection; // Hướng bay ra
    private float nextFireTime;

    public enum AttackPattern { Circular, Vertical }
    private AttackPattern currentPattern;
    private Vector3 movementDirection; // Hướng bắn

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;  
        transform.position = target + new Vector3(0, 5, 0);  // Dịch minion lên trên trước khi rơi xuống
    }

    public void SetCircularPattern(Vector3 bossPosition)
    {
        currentPattern = AttackPattern.Circular;

        // **Sửa lại: Tính từ targetPosition chứ không phải transform.position**
        Vector3 directionFromBoss = (targetPosition - bossPosition);
        float angle = Mathf.Atan2(directionFromBoss.y, directionFromBoss.x) * Mathf.Rad2Deg;

        // Xoay minion để nhìn ra xa boss
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Cập nhật hướng di chuyển (hướng mà minion sẽ bắn)
        movementDirection = directionFromBoss.normalized;

        // Kiểm tra nếu movementDirection có bị sai không
        Debug.Log("Minion movementDirection: " + movementDirection);

        // Bắt đầu bắn
        StartCoroutine(FireBullets());
    }



    public void SetVerticalPattern(int waypointIndex)
    {
        currentPattern = AttackPattern.Vertical;

        if (waypointIndex % 3 == 2)
        {
            movementDirection = Vector3.right;  // Bắn từ trái sang phải
        }
        else if (waypointIndex % 3 == 1)
        {
            movementDirection = Vector3.left;   // Bắn từ phải sang trái
        }
        else
        {
            movementDirection = Vector3.down;   // Bắn từ trên xuống dưới
        }

        StartCoroutine(FireBullets());
    }


    IEnumerator FireBullets()
    {
        while (lifetime > 3f)
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                Fire();
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    void Fire()
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = movementDirection * bulletSpeed;
            }
        }
    }

    void Update()
    {   
        void Update()
        {
            // Hiển thị hướng up (thường là hướng trên)
            Debug.DrawRay(transform.position, transform.up * 2, Color.green);

            // Hiển thị hướng right (thường là hướng phải)
            Debug.DrawRay(transform.position, transform.right * 2, Color.red);
        }


        if (isEntering)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, enterSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isEntering = false;
                StartCoroutine(StartExitSequence());
            }
        }
        else if (!isEntering && lifetime <= 3f)
        {
            transform.position += exitDirection * exitSpeed * Time.deltaTime;
        }
        
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator StartExitSequence()
    {
        yield return new WaitForSeconds(5f); // Tấn công trong 5 giây trước khi rời đi
        exitDirection = new Vector3(0, 1, 0); // Bay lên trên màn hình
        isEntering = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBulletTag"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}

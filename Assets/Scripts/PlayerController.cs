using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject GameManagerGO; // Reference to GameManager
    public GameObject PlayerBulletGO; // Player's bullet prefab
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject ExplosionGO; // Explosion prefab
    public GameManager gameOver;
    public GameManager HP;
    public GameObject collisionEffectPrefab;
    public float currentHP;
    public float maxHP = 10;
    public float speed;

    private bool isDead;
    private bool canMove = true; // Biến kiểm tra Player có thể di chuyển

    void Start()
    {
        currentHP = maxHP;

        if (HP != null)
        {
            HP.UpdateHP(currentHP, maxHP);
        }
        else
        {
            Debug.LogError("⚠ Lỗi: Biến HP chưa được gán trong PlayerController!");
        }
    }

    void Update()
    {
        if (canMove) // Chỉ di chuyển nếu không bị dừng
        {
            if (Input.GetKeyDown("space"))
            {
                GetComponent<AudioSource>().Play();

                GameObject bullet01 = Instantiate(PlayerBulletGO);
                bullet01.transform.position = bulletPosition01.transform.position;

                GameObject bullet02 = Instantiate(PlayerBulletGO);
                bullet02.transform.position = bulletPosition02.transform.position;
            }

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            Vector2 direction = new Vector2(x, y).normalized;
            Move(direction);
        }
    }

    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x -= 0.225f;
        min.x += 0.225f;
        max.y -= 0.225f;
        min.y += 0.225f;

        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyShipTag") || col.CompareTag("EnemyBulletTag"))
        {
            TakeDamage(2f);
            PlayExplosion();
        }

        if (col.CompareTag("Boss")) // 🔥 Va chạm Boss, mất 2 HP
        {
            Debug.Log("🔥 Player va chạm với Boss, mất 2 HP!");

            // Xác định hướng va chạm
            Vector2 collisionDirection = (transform.position - col.transform.position).normalized;

            // Tạo hiệu ứng tại điểm va chạm
            Vector3 hitPosition = (transform.position + col.transform.position) / 2;
            CreateCollisionEffect(hitPosition);

            // Đẩy tàu lùi lại
            StartCoroutine(Knockback(collisionDirection, 0.3f, 2f));

            TakeDamage(2f);
        }

        if (col.CompareTag("Meteor"))
        {
            StartCoroutine(FreezePlayer(2f)); 
        }

        if (col.CompareTag("Meteor2"))
        {
            StartCoroutine(FreezePlayer(3f)); 
        }
    }



    void CreateCollisionEffect(Vector3 position)
    {
        if (collisionEffectPrefab != null)
        {
            GameObject effect = Instantiate(collisionEffectPrefab, position, Quaternion.identity);
            Destroy(effect, 0.5f); // Xóa sau 0.5s để tránh rác bộ nhớ
        }
    }


    IEnumerator FreezePlayer(float duration)
    {
        Debug.Log("Bắt đầu đóng băng Player trong " + duration + " giây");
        canMove = false; // Ngăn Player di chuyển
        yield return new WaitForSeconds(duration); // Đợi theo thời gian truyền vào
        canMove = true; // Cho phép Player di chuyển lại
        Debug.Log("Player có thể di chuyển lại");
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        HP.UpdateHP(currentHP, maxHP);

        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            gameOver.Over();
            Destroy(gameObject);
        }
    }

    IEnumerator Knockback(Vector2 direction, float duration, float force)
    {
        float timer = 0;
        canMove = false; // Tạm thời vô hiệu hóa di chuyển

        while (timer < duration)
        {
            transform.position += (Vector3)direction * force * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        canMove = true; // Cho phép di chuyển lại
    }


    void PlayExplosion()
    {
        GameObject explosion = Instantiate(ExplosionGO);
        explosion.transform.position = transform.position;
    }
}
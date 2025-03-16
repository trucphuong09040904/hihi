using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 100;

    void Update()
    {
        // Di chuyển viên đạn
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Nếu viên đạn đi ra khỏi màn hình, hủy nó
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Nếu bắn trúng Boss màn 5
        if (col.CompareTag("Boss"))
        {
            BossAI boss = col.GetComponent<BossAI>();
            if (boss != null)
            {
                boss.TakeDamage(damage); // Trừ máu Boss
            }
            Destroy(gameObject);
        }
        // Nếu bắn trúng Boss màn 6
        else if (col.CompareTag("BossM6"))
        {
            BossController boss = col.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(damage); // Trừ máu Boss
            }
            Destroy(gameObject);
        }
        // Nếu bắn trúng Phân thân của Boss màn 6
        else if (col.CompareTag("Minion"))
        {
            BossMinionController clone = col.GetComponent<BossMinionController>();
            if (clone != null)
            {
               
            }
            Destroy(gameObject);
        }
    }
}

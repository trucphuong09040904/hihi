using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    float speed;
    public GameObject explosionEffect; // Hiệu ứng nổ

    void Start()
    {
        speed = 8f;
    }

    void Update()
    {
        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);
        transform.position = position;

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyShipTag")) 
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }

        if (col.CompareTag("Boss")) 
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            // Gọi TakeDamage bên Boss, truyền vị trí va chạm
            BossAI boss = col.GetComponent<BossAI>();
            if (boss != null)
            {
                boss.TakeDamage(100, transform.position);
            }

            Destroy(gameObject);
        }
    }

}

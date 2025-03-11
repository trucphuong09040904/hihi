using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor2 : MonoBehaviour
{
    public float speed = 3f; // Tốc độ rơi của thiên thạch

    void Update()
    {
        // Cập nhật vị trí rơi xuống
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Hủy thiên thạch nếu rơi ra khỏi màn hình
        if (transform.position.y < Camera.main.ViewportToWorldPoint(Vector2.zero).y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerShipTag") || col.CompareTag("PlayerBulletTag"))
        {
            Destroy(gameObject); // Hủy thiên thạch nếu va chạm với tàu hoặc đạn
        }
    }
}

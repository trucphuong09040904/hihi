using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Nếu đạn bay ra ngoài màn hình thì destroy
        if (!IsVisible())
        {
            Destroy(gameObject);
        }
    }

    bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShipTag"))
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab; // Prefab thiên thạch
    public float minSpawnTime = 1f; // Thời gian sinh tối thiểu
    public float maxSpawnTime = 3f; // Thời gian sinh tối đa

    void Start()
    {
        StartCoroutine(SpawnMeteorRoutine());
    }

    IEnumerator SpawnMeteorRoutine()
    {
        while (true) // Lặp vô hạn để tạo thiên thạch liên tục
        {
            SpawnMeteor();
            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnDelay); // Chờ một khoảng thời gian trước khi tạo thiên thạch tiếp theo
        }
    }

    void SpawnMeteor()
    {
        // Xác định giới hạn màn hình
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Tạo thiên thạch ở vị trí ngẫu nhiên trên đầu màn hình
        Vector2 spawnPosition = new Vector2(Random.Range(min.x, max.x), max.y);
        Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
    }
}

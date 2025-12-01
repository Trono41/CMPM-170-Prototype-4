using UnityEngine;

public class TornadoRaycastSpawner : MonoBehaviour
{
    public GameObject tornadoPrefab;
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 8f;
    private float nextSpawnTime;

    public Transform leftBound;
    public Transform rightBound;

    public LayerMask groundLayer;
    public float raycastHeight = 10f;

    public int tornadoesPerCycle = 3;

    public float verticalSpawnOffset = 1.0f;

    void Start()
    {
        if (leftBound == null || rightBound == null)
        {
            Debug.LogError("Spawner Bounds not assigned! Disabling spawner.");
            enabled = false;
            return;
        }
        SetNextSpawnTime();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnTornado();
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnTornado()
    {
        for (int i = 0; i < tornadoesPerCycle; i++)
        {
            float randomX = Random.Range(leftBound.position.x, rightBound.position.x);
            Vector2 rayStart = new Vector2(randomX, transform.position.y + raycastHeight);

            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, Mathf.Infinity, groundLayer);

            if (hit.collider != null)
            {
                Vector3 spawnPosition = new Vector3(randomX, hit.point.y + verticalSpawnOffset, 0f);
                Instantiate(tornadoPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

public class TornadoRaycastSpawner : MonoBehaviour
{
    public GameObject tornadoPrefab;
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 8f;
    private float nextSpawnTime;

    public Transform leftBound;
    public Transform rightBound;

    public LayerMask groundLayer;
    public float raycastStartHeight = 50f;

    public int tornadoesPerCycle = 3;
    public float minHorizontalSpacing = 5f;
    public float minVerticalSpacing = 3f;

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
        List<Vector3> allPossibleSpawnPoints = new List<Vector3>();
        int scanCount = 20;

        for (int scan = 0; scan < scanCount; scan++)
        {
            float randomX = Random.Range(leftBound.position.x, rightBound.position.x);
            Vector2 rayStart = new Vector2(randomX, raycastStartHeight);

            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, Mathf.Infinity, groundLayer);

            if (hit.collider != null)
            {
                Vector3 spawnPoint = new Vector3(randomX, hit.point.y + verticalSpawnOffset, 0f);
                allPossibleSpawnPoints.Add(spawnPoint);
            }
        }

        if (allPossibleSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No valid spawn points found!");
            return;
        }

        List<List<Vector3>> heightLevels = GroupByHeight(allPossibleSpawnPoints, minVerticalSpacing);

        List<Vector3> chosenSpawnPositions = new List<Vector3>();
        List<int> usedLevelIndices = new List<int>();

        for (int i = 0; i < tornadoesPerCycle; i++)
        {
            Vector3 spawnPosition = Vector3.zero;
            bool validPosition = false;
            int attempts = 0;
            int maxAttempts = 30;

            while (!validPosition && attempts < maxAttempts)
            {
                attempts++;

                int levelIndex = -1;
                if (usedLevelIndices.Count < heightLevels.Count)
                {
                    do
                    {
                        levelIndex = Random.Range(0, heightLevels.Count);
                    } while (usedLevelIndices.Contains(levelIndex));
                }
                else
                {
                    levelIndex = Random.Range(0, heightLevels.Count);
                }

                List<Vector3> levelPoints = heightLevels[levelIndex];
                if (levelPoints.Count == 0) continue;

                Vector3 potentialPosition = levelPoints[Random.Range(0, levelPoints.Count)];

                validPosition = true;
                foreach (Vector3 existingPos in chosenSpawnPositions)
                {
                    float horizontalDistance = Mathf.Abs(potentialPosition.x - existingPos.x);
                    float verticalDistance = Mathf.Abs(potentialPosition.y - existingPos.y);

                    if (verticalDistance < minVerticalSpacing && horizontalDistance < minHorizontalSpacing)
                    {
                        validPosition = false;
                        break;
                    }
                }

                if (validPosition)
                {
                    spawnPosition = potentialPosition;
                    chosenSpawnPositions.Add(spawnPosition);
                    if (!usedLevelIndices.Contains(levelIndex))
                    {
                        usedLevelIndices.Add(levelIndex);
                    }
                }
            }

            if (spawnPosition != Vector3.zero)
            {
                GameObject tornado = Instantiate(tornadoPrefab, spawnPosition, Quaternion.identity);

                TornadoController controller = tornado.GetComponent<TornadoController>();
                if (controller != null)
                {
                    controller.leftBound = leftBound;
                    controller.rightBound = rightBound;
                }
            }
        }
    }

    List<List<Vector3>> GroupByHeight(List<Vector3> points, float heightThreshold)
    {
        List<List<Vector3>> levels = new List<List<Vector3>>();

        foreach (Vector3 point in points)
        {
            bool addedToLevel = false;

            foreach (List<Vector3> level in levels)
            {
                if (level.Count > 0)
                {
                    float heightDiff = Mathf.Abs(point.y - level[0].y);
                    if (heightDiff < heightThreshold)
                    {
                        level.Add(point);
                        addedToLevel = true;
                        break;
                    }
                }
            }

            if (!addedToLevel)
            {
                List<Vector3> newLevel = new List<Vector3> { point };
                levels.Add(newLevel);
            }
        }

        levels.Sort((a, b) => b[0].y.CompareTo(a[0].y));

        return levels;
    }
}
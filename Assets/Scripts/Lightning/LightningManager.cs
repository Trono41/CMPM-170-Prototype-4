using System.Collections;
using UnityEngine;

public class LightningManager : MonoBehaviour
{
    private static LightningManager _instance;
    public static LightningManager Instance { get { return _instance; } }
 
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject lightningBolt;
    [SerializeField] private float yOffset = 0.5f;
    public float lightningStrikesPerCycle;
    public float lightningStrikeCycleTime;
    
    private GameObject[] floorTiles;
    
    private void Awake()
    {
        // Singleton enforcement
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        // grab floor boxes and save them
        floorTiles = GameObject.FindGameObjectsWithTag("Floor");
        
        StartCoroutine("SpawnLightningIndicator");
    }

    IEnumerator SpawnLightningIndicator()
    {
        while (true)
        {

            for (int i = 0; i < lightningStrikesPerCycle; i++)
            {
                Vector3 spawnPos = GetLightningLocation();

                GameObject bolt = Instantiate(indicator, spawnPos, Quaternion.identity);
                bolt.GetComponent<LightningIndicator>().caller = this;
            }

            yield return new WaitForSeconds(lightningStrikeCycleTime);
        }
    }

    private Vector3 GetLightningLocation()
    {
        if (floorTiles == null || floorTiles.Length == 0)
        {
            Debug.LogError("No floor tiles found!");
            return Vector3.zero;
        }

        // pick a random floor tile
        GameObject floor = floorTiles[Random.Range(0, floorTiles.Length)];
        BoxCollider2D col = floor.GetComponent<BoxCollider2D>();

        if (col == null)
        {
            Debug.LogError("Floor tile missing BoxCollider2D!");
            return Vector3.zero;
        }

        Bounds b = col.bounds;
        
        // random point across the tile
        float randX = Random.Range(b.min.x, b.max.x);

        // Spawn directly above the top of the collider
        float spawnY = b.max.y + yOffset;

        return new Vector3(randX, spawnY, 0);
    }

    public void LightningStrike(Vector3 position)
    {
        Instantiate(lightningBolt, position, Quaternion.identity);
    }
    
}

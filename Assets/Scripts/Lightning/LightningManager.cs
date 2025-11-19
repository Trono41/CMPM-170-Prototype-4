using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LightningManager : MonoBehaviour
{
    private static LightningManager _instance;
    public static LightningManager Instance { get { return _instance; } }
 
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject lightningBolt;
    [SerializeField] private Vector2 lightningPosition;
    
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
        StartCoroutine("SpawnLightningIndicator");
    }

    IEnumerator SpawnLightningIndicator()
    {
        while (true)
        {
            GameObject bolt = Instantiate(indicator, GetLightningLocation(), Quaternion.identity);
            bolt.GetComponent<LightningIndicator>().caller = this;
            
            yield return new WaitForSeconds(6f);
        }
    }

    private Vector3 GetLightningLocation()
    {
        // will be changed to consider platforms
        return lightningPosition;
    }

    public void LightningStrike(Vector3 position)
    {
        Instantiate(lightningBolt, position, Quaternion.identity);
    }
    
}

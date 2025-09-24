using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] hazardPrefabs; // rocks, boats
    public GameObject[] pickupPrefabs;

    public float initialInterval = 3f;
    public float minInterval = 0.6f;
    public float difficultyRamp = 0.02f; // how quickly interval shortens per second

    float currentInterval;

    void Start()
    {
        currentInterval = initialInterval;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentInterval);
            TrySpawn();
            currentInterval = Mathf.Max(minInterval, currentInterval - difficultyRamp);
        }
    }

    void TrySpawn()
    {
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        float roll = Random.value;
        if (roll < 0.65f)
        {
            var prefab = hazardPrefabs[Random.Range(0, hazardPrefabs.Length)];
            Instantiate(prefab, spawn.position, Quaternion.identity);
        }
        else
        {
            var prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
            Instantiate(prefab, spawn.position, Quaternion.identity);
        }
    }
}

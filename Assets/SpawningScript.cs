using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningScript : MonoBehaviour
{
    public GameObject healthBuffPrefab; // Reference to the prefab to spawn for the health buff
    public float spawnDelay = 10f; // Delay before spawning the prefab

    

    // Call this method to spawn the health buff prefab after a delay
    public void SpawnHealthBuffPrefabDelayed()
    {
        Debug.Log("Health buff spawning in " + spawnDelay + " seconds.");
        Invoke("SpawnHealthBuffPrefab", spawnDelay);
    }

    // Spawn the health buff prefab
    private void SpawnHealthBuffPrefab()
    {
        // Spawn the prefab
        Instantiate(healthBuffPrefab, transform.position, Quaternion.identity);
    }
}

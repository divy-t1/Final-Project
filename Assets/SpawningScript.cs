using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningScript : MonoBehaviour
{
    public GameObject healthBuffPrefab; // The value reference to the health buff prefab
    public float spawnDelay = 10f; // Delay before spawning the prefab

    

    // Calling this method to spawn the health buff prefab after a delay
    public void SpawnHealthBuffPrefabDelayed()
    {
        Debug.Log("Health buff spawning in " + spawnDelay + " seconds.");
        Invoke("SpawnHealthBuffPrefab", spawnDelay);
    }

    //Method to spawn the health buff prefab
    private void SpawnHealthBuffPrefab()
    {
        // Spawn the prefab
        Instantiate(healthBuffPrefab, transform.position, Quaternion.identity);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
    
    public Transform[] spawnPoints; // an array of the transform portion of the spawnpoints  
    public PlayerHealth playerHealth;
    public Transform playerTransform;
    public List<SpawnableObject> spawnableObjects; // List of spawnable objects

    [Serializable]
    public class SpawnableObject
    {
        public GameObject prefab; // The prefab to spawn
        public int initialPrefabCount; // The initial number of prefabs to spawn
        public int currentPrefabCount; // The current number of prefabs in the scene
        public List<GameObject> instances; // Track spawned instances

        public void InitializeInstancesList()
        {
            instances = new List<GameObject>();
        }
    }

    void Start() {
        // Initialize and spawn the initial prefabs for each spawnable object
        foreach (SpawnableObject spawnableObject in spawnableObjects)
        {
            spawnableObject.InitializeInstancesList();
            for (int i = 0; i < spawnableObject.initialPrefabCount; i++)
            {
                SpawnPrefab(spawnableObject);
            }
        }

        LoadPlayerData(); 
    } 

    // Method to save player data
    public void SavePlayerData()
    {
        SaveSystem.SavePlayer(this);
    }
    

    public void LoadPlayerData()
    {
        PlayerData loadedData = SaveSystem.LoadPlayer();
        if (loadedData != null)
        {
            playerHealth.SetHealth(loadedData.health);

            // Set player position from loaded data
            Vector3 position = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
            playerTransform.position = position;

            // Other data loading operations...
        }
        else
        {
            Debug.LogError("Failed to load player data.");
        }
    }

    void SpawnPrefab(SpawnableObject spawnableObject)
    {
        if (spawnableObject.currentPrefabCount >= spawnableObject.initialPrefabCount)
        {
            Debug.Log("Limit reached for " + spawnableObject.prefab.name + ". No need to spawn more prefabs.");
            return; // Limit reached, no need to spawn any more prefabs
        }

        // Randomly select a spawn point from the array
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        // Instantiate the prefab at the spawn point
        GameObject newPrefab = Instantiate(spawnableObject.prefab, spawnPoint.position, Quaternion.identity);
        ISpawnable spawnableComponent = newPrefab.GetComponent<ISpawnable>(); // Use interface

        if (spawnableComponent != null)
        {
            spawnableComponent.Initialize(this, spawnableObject); // Call method to initialize
        }

        // Add the new instance to the list and increment the count
        spawnableObject.instances.Add(newPrefab);
        spawnableObject.currentPrefabCount++; // Increase the count of the prefabs by one
        Debug.Log("Spawned " + spawnableObject.prefab.name + ". Current count: " + spawnableObject.currentPrefabCount);
    }

    public void PrefabDestroyed(SpawnableObject spawnableObject, GameObject prefabInstance)
    {
        spawnableObject.currentPrefabCount--; // Decrease the current count by one
        spawnableObject.instances.Remove(prefabInstance); // Remove the instance from the list
        Debug.Log(spawnableObject.prefab.name + " destroyed. Current count: " + spawnableObject.currentPrefabCount);
        SpawnPrefab(spawnableObject); // Spawn another prefab to maintain the count
    }
    

} 

public interface ISpawnable
{
    void Initialize(GameManager gameManager, GameManager.SpawnableObject spawnableObject);
}




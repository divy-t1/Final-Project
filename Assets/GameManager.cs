using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
    public GameObject prefab; // the prefab that should respawn 
    public Transform[] spawnPoints; // an array of the transform portion of the spawnpoints 
    public int initialPrefabCount; // the count that the prefab should maintain and starts at 
    public int currentPrefabCount; // the count that changes and is monitered 
    public PlayerHealth playerHealth;
    public Transform playerTransform;

    void Start() {
        // spawn the initial prefabs 
        for (int i = 0; i < initialPrefabCount; i++) {
            SpawnPrefab(); 
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

    void SpawnPrefab() {
        if (currentPrefabCount >= initialPrefabCount)
        {
            return; // Limit reached, no need to spawn more prefabs
        }

        // Randomly select a spawn point from the array
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        // Instantiate the health buff prefab at the spawn point
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        currentPrefabCount++; // Increase the count of the prefabs by one
    }

    public void PrefabDestroyed() {
        currentPrefabCount--; // decrease the current count by one to show a prefab has been destroyed
        SpawnPrefab(); // call the function to spawn again 
    }
    

} 

/*
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
     /*
     a public class in which each object created from it will represent a spawnable object that 
     has a defined connected prefab, a defined initial count and current count. When you create
     the list that holds these objects, each will have its own prefab, initial count, and current
     count that can be manipulated.  
     
    [Serializable]     
    public class SpawnableObject {
        public GameObject prefab; // the prefab that should respawn 
        public int initialPrefabCount; // the count that the prefab should maintain and starts at 
        public int currentPrefabCount; // the count that changes and is monitered, represents how many are in scene at the moment
        public GameObject[] instances; // Keep track of spawned instances

        public void InitializeInstancesArray()
        {
            instances = new GameObject[initialPrefabCount];
        }
    }


    public Transform[] spawnPoints; // an array of the transform portion of the spawnpoints 
    public List<SpawnableObject> spawnableObjects; // a list that will hold the spawnable objects

    void Start() {
        // spawn the initial prefabs for each spawnable object 
        foreach (SpawnableObject spawnableObject in spawnableObjects) {
            spawnableObject.InitializeInstancesArray(); // Initialize the instances array
            for (int i = 0; i < spawnableObject.initialPrefabCount; i++) {
                SpawnPrefab(spawnableObject);  
            }
        }
        
    } 

    void SpawnPrefab(SpawnableObject spawnableObject) {
        if (spawnableObject.currentPrefabCount >= spawnableObject.initialPrefabCount)
        {
            Debug.Log("Limit reached for " + spawnableObject.prefab.name + ". No need to spawn more prefabs.");
            return; // Limit reached, no need to spawn any more prefabs
        }

        // Randomly select a spawn point from the array
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        // Instantiate the health buff prefab at the spawn point
        GameObject newPrefab = Instantiate(spawnableObject.prefab, spawnPoint.position, Quaternion.identity); 
        ISpawnable spawnableComponent = newPrefab.GetComponent<ISpawnable>(); // Use interface

        if (spawnableComponent != null) {
            spawnableComponent.Initialize(this, spawnableObject); // Call method to initialize
        }

        if (spawnableObject.instances != null && spawnableObject.currentPrefabCount < spawnableObject.instances.Length) {
            // Add the new instance to the array and then increment the count
            spawnableObject.instances[spawnableObject.currentPrefabCount] = newPrefab;
            spawnableObject.currentPrefabCount++; // Increase the count of the prefabs by one
            Debug.Log("Spawned " + spawnableObject.prefab.name + ". Current count: " + spawnableObject.currentPrefabCount);
        } else {
            Debug.LogError("Instances array is not properly initialized or index is out of bounds.");
        }
    } 

    public void PrefabDestroyed(SpawnableObject spawnableObject) {
        spawnableObject.currentPrefabCount--; // decrease the current count by one to show a prefab has been destroyed
        Debug.Log(spawnableObject.prefab.name + " destroyed. Current count: " + spawnableObject.currentPrefabCount);
        SpawnPrefab(spawnableObject); // call the function to spawn again 
    }

    

}
*/


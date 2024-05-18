using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

//Define a simple class to hold the information needed for each type of object.
[Serializable]
class ObjectData {
    public GameObject prefab;
    public int initialPrefabCount;
    public int currentPrefabCount;
}

[Serializable]
public class GameManager : MonoBehaviour
{


    public PlayerHealth playerHealth;
    public Transform playerTransform;
    [SerializeField] private List<ObjectData> mazeObjects;
    public Transform[] spawnPoints; // an array of the transform portion of the spawnpoints 
    private List<Vector3> availableSpawnPoints = new List<Vector3>();


    void Start() {
        InitializeSpawnPoints(); 
        InitializeMazeObjects(); 
        LoadPlayerData(); 
    } 

    // Method to initialze the list of available spawn points 
    private void InitializeSpawnPoints() {
        // Loop through each spawn point in the array of transform components 
        foreach (var spawnPoint in spawnPoints) {
            // Add the position of each spawn point to the list of the available spawn points
            availableSpawnPoints.Add(spawnPoint.position); 
        }
        Debug.Log("Initialized " + availableSpawnPoints.Count + " spawn points.");
    }

    // Initialize and spawn the initial prefabs for each spawnable object
    private void InitializeMazeObjects() {
        // Loop through each objectdata in the maze object list 
        for (int i = 0; i < mazeObjects.Count; i++) {
            // Reset currentPrefabCount to ensure correct initialization
            mazeObjects[i].currentPrefabCount = 0;
            // Spawn the initial number of prefabs specified for each object type 
            SpawnPrefabRecursive(i);
        }
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

    void SpawnPrefabRecursive(int index)
    {
        // Base Case 1: check to see if the current number of prefab has reached the initial count
        if (mazeObjects[index].currentPrefabCount >= mazeObjects[index].initialPrefabCount)
        {
            Debug.Log("Limit reached for " + mazeObjects[index].prefab.name + ". No need to spawn more prefabs.");
            return; // Limit reached, no need to spawn any more prefabs
        }

        // Base Case 2: if there are no spawn points left 
        if (availableSpawnPoints.Count == 0) {
            Debug.Log("No available spawn points left"); 
            return; // no need to spawn anything 
        }

        // Randomly select a spawn point from the array
        int randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
        Vector3 spawnPoint = availableSpawnPoints[randomIndex]; 
        availableSpawnPoints.RemoveAt(randomIndex);

        // Instantiate the prefab at the spawn point
        GameObject newPrefab = Instantiate(mazeObjects[index].prefab, spawnPoint, Quaternion.identity);

        //increase the count of the prefabs by one 
        mazeObjects[index].currentPrefabCount++; 
        
        //When a new object is created, GameManager needs to give the object a reference
        //to this GameManager and tell the object which index of mazeObject is being
        //used to track its type of object.
        //This lets the spawned object properly call PrefabDestroyed later.
        IMazeObject newMazeObject = newPrefab.GetComponent<IMazeObject>();
        newMazeObject.gameManager = this;
        newMazeObject.ObjectIndex = index;
        Debug.Log("Spawned " + mazeObjects[index].prefab.name + ". Current count: " + mazeObjects[index].currentPrefabCount);

        // Recursive call to continue spawning 
        SpawnPrefabRecursive(index); 
    }

    public void PrefabDestroyed(int index, Vector3 position)
    {
        mazeObjects[index].currentPrefabCount--; // Decrease the current count by one
        Debug.Log(mazeObjects[index].prefab.name + " destroyed. Current count: " + mazeObjects[index].currentPrefabCount);

        // Spawn another prefab to maintain the count 
        SpawnPrefabRecursive(index); 

        //Don't add the previous position to the available list until after spawning
        //the new object. Otherwise there is a change you will spawn in exactly the same
        //place and the player will immediately collect it.
        availableSpawnPoints.Add(position);


    }
    

} 






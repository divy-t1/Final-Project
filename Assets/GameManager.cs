using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
    
    public PlayerHealth playerHealth;
    public Transform playerTransform;
    [SerializeField] private List<ObjectData> mazeObjects;
    public Transform[] spawnPoints; // an array of the transform portion of the spawnpoints 

    //Define a simple class to hold the information needed for each type of object.
    [Serializable]
    class ObjectData
    {
        public GameObject prefab;
        public int initialPrefabCount;
        public int currentPrefabCount;
    }

    void Start() {
        // Initialize and spawn the initial prefabs for each spawnable object
        for (int i = 0; i < mazeObjects.Count; i++) {

            for (int j = 0; j < mazeObjects[i].initialPrefabCount; j++) {
                SpawnPrefab(i);
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

    void SpawnPrefab(int index)
    {
        if (mazeObjects[index].currentPrefabCount >= mazeObjects[index].initialPrefabCount)
        {
            Debug.Log("Limit reached for " + mazeObjects[index].prefab.name + ". No need to spawn more prefabs.");
            return; // Limit reached, no need to spawn any more prefabs
        }

        // Randomly select a spawn point from the array
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        // Instantiate the prefab at the spawn point
        GameObject newPrefab = Instantiate(mazeObjects[index].prefab, spawnPoint.position, Quaternion.identity);

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
    }

    public void PrefabDestroyed(int index)
    {
        mazeObjects[index].currentPrefabCount--; // Decrease the current count by one
        Debug.Log(mazeObjects[index].prefab.name + " destroyed. Current count: " + mazeObjects[index].currentPrefabCount);
        SpawnPrefab(index); // Spawn another prefab to maintain the count
    }
    

} 






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

    void Start() {
        // spawn the initial prefabs 
        for (int i = 0; i < initialPrefabCount; i++) {
            SpawnPrefab(); 
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

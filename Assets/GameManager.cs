using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
    public class SpawnableObject {
        public GameObject prefab; // prefab of the object we want to spawn 
        public int initialCount; // inital count that we want to maintain of the prefab 
        public int currentCount; // current count of the prefab 
    }

    public SpawnableObject[] spawnableObjects; // an array that holds the different spawnable objects 
    public Transform[] spawnPoints; // the array that holds all the spawn points a prefab can generate at 

    void Start() {
        // fine all game objects in scene with the tag 'spawn point' 
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint"); 

        // initialize them into the spawn points array with the transform part of the game objects 
        spawnPoints = new Transform[spawnPointObjects.Length]; 
        for (int i = 0; i < spawnPointObjects.Length; i++) {
            spawnPoints[i] = spawnPointObjects[i].transform; 
        }

        // this loop will spawn the initial count of each spawnable object 
        foreach (SpawnableObject spawnableObject in spawnableObjects) {
            for (int i = 0; i < spawnableObject.initialCount; i++) {
                SpawnObject(spawnableObject); 
            }
        }
    }

    void SpawnObject(SpawnableObject spawnableObject) {
        //checks to see the amount of prefabs of the object is less than or equal to the initial count 
        if (spawnableObject.currentCount >= spawnableObject.initialCount) {
            return; // if its greater than, we return nothing, so the spawn doesn't happen 
        } 

        //Randomly select a spawn point from the array of Spawn points 
        Transform selectedSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)]; 

        // instantiate a new prefab at the randomly selected point from the spawn points array 
        Instantiate(spawnableObject.prefab, selectedSpawnPoint.position, Quaternion.identity); 

        // adding to the current count to keep track 
        spawnableObject.currentCount++; 

        // recursive function where it'll keep calling till initial count is reached 
        if (spawnableObject.currentCount <= spawnableObject.initialCount) {
            SpawnObject(spawnableObject); 
        }
    }

    public void ObjectDestroyed(SpawnableObject spawnableObject) {
        spawnableObject.currentCount--; //decrement count to show lack of required prefabs 
        SpawnObject(spawnableObject); // call function to spawn it again 
    }


}

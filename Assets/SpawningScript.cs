using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawningScript : MonoBehaviour
{
    public GameObject targetObject; //object that player interacts with
    public float delay; //the delay between spawning the objects
    public Transform[] spawnPoints; //the position array of the spawn points around the maze
    //private Transform selectedSpawnPoint; 

 
    void Start()
    { // checks whether any reference in the game editor is null
        if (targetObject == null)
        {
            Debug.LogWarning("TargetObject reference is not assigned.");
        }
        if (delay == 0)
        {
            Debug.LogWarning("Delay reference is not assigned.");
        }
        if (spawnPoints == null)
        {
            Debug.LogWarning("SpawnPoints reference is not assigned.");
        } 
    }

    public void Spawn() {
        Debug.Log("Spawn method called"); 

        //get a random spawn point
        Transform selectedSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)]; 
        
        /*
        for (int i = 0; i < spawnPoints.Length; i++) {
            selectedSpawnPoint = spawnPoints[i]; 
        } 
        
        Vector3 position; 
        position.x = selectedSpawnPoint.position.x; 
        position.y = selectedSpawnPoint.position.y; 
        position.z = selectedSpawnPoint.position.z; 
        */

        //get the position from that spawn point
        Vector3 position = selectedSpawnPoint.position; 

        //send an error to track whether spawn even happened or not
        Debug.Log("Spawned at " + selectedSpawnPoint.name); 
        //spawn the same target object to the random position we got just now 
        Instantiate(FindObjectOfType<Manager>().healthSpawn, position, Quaternion.identity); 

    }
}

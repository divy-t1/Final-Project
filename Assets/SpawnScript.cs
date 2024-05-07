using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
   public GameObject objectToSpawn; 

   public float timeToSpawn; 

   private float currentTimeToSpawn; 

   void Start() {
    spawnObject(); 
   } 

   void Update() {
    if(currentTimeToSpawn > 0) {
            currentTimeToSpawn -= Time.deltaTime; 
        } else {
            spawnObject(); 
            currentTimeToSpawn = timeToSpawn; 
        }
   }

   public void spawnObject() {
    Instantiate(objectToSpawn, transform.position, Quaternion.identity); 
   }
}


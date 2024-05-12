using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TrapTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;  
    public HealthBar healthBar; 
    public PlayerHealth playerHealth; 
    public GameObject trapPrefab; 
    //private bool isTriggered; 

    void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth reference is not assigned.");
        }
        if (healthBar == null)
        {
            Debug.LogWarning("HealthBar reference is not assigned.");
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //if(!isTriggered && collision.gameObject.tag == "Player"){
        if(collision.gameObject.tag == "Player"){
            Debug.Log("Player collided with a " + gameObject.name);
            if (playerHealth != null && healthBar != null) {
                playerHealth.currentHealth -= damage; 
                healthBar.SetHealth(playerHealth.currentHealth); 
                collision.transform.position = new Vector3(1.55f, -0.22f, 0); 
            } else {
                Debug.LogError("PlayerHealth or HealthBar reference is null.");
            }
            Destroy(gameObject);
            
            /* Spawn the trap prefab immediately by checking if the field for prefab is null or not
            if (trapPrefab != null) {
                SpawnTrapPrefab();
            } else {
                Debug.LogError("TrapPrefab reference is null.");
            }

            // Debug message for collision and spawning
            Debug.Log("Trap triggered by: " + collision.gameObject.name);
            Debug.Log("Trap spawned immediately."); */

            //isTriggered = true; 
        }
    }

    private void SpawnTrapPrefab()
    {
        // Instantiate the health buff prefab at the location of the prefab itself
        GameObject spawnedTrap = Instantiate(trapPrefab, transform.position, Quaternion.identity);

        // Debug message after spawning
        if (spawnedTrap != null) {
            Debug.Log("Health buff spawned at: " + spawnedTrap.transform.position);
        } else {
            Debug.LogError("Failed to spawn the trap prefab!");
        }
    }
}

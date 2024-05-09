using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class HealthBuff : MonoBehaviour
{
    public int amount; 
    public HealthBar healthBar; 
    public PlayerHealth playerHealth; 
    public GameObject healthBuffPrefab; 
    public float delay; 
 
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
        if (healthBuffPrefab == null)
        {
            Debug.LogWarning("HealthBuffPrefab reference is not assigned.");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        
        if(collision.gameObject.tag == "Player"){
            Debug.Log("Player collided with health buff.");
            if (playerHealth != null && healthBar != null) {
                playerHealth.currentHealth += amount; 
                healthBar.SetHealth(playerHealth.currentHealth); 
            } else {
                Debug.LogError("PlayerHealth or HealthBar reference is null.");
            }
            Destroy(gameObject);
            
            // Spawn the health buff prefab immediately
            if (healthBuffPrefab != null) {
                SpawnHealthBuffPrefab();
            } else {
                Debug.LogError("HealthBuffPrefab reference is null.");
            }

            // Debug message for collision and spawning
            Debug.Log("Health buff triggered by: " + collision.gameObject.name);
            Debug.Log("Health buff spawned immediately.");
        }
    }

    private void SpawnHealthBuffPrefab()
    {
        // Instantiate the health buff prefab at the desired location
        GameObject spawnedHealthBuff = Instantiate(healthBuffPrefab, transform.position, Quaternion.identity);

        // Debug message after spawning
        if (spawnedHealthBuff != null)
        {
            Debug.Log("Health buff spawned at: " + spawnedHealthBuff.transform.position);
        }
        else
        {
            Debug.LogError("Failed to spawn health buff prefab!");
        }
    }


    
}

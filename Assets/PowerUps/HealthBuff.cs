using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class HealthBuff : MonoBehaviour
{
    public int amount; 
    //the amount that the health bar has added
    public HealthBar healthBar; 
    //reference to the healthbar that changes with teh collision
    public PlayerHealth playerHealth; 
    //reference to the player health system of the player it collides with
    public GameManager gameManager; 
    
    
   
 
    void Start()
    { // checks whether any reference in the game editor is null
        if (playerHealth == null) {
            playerHealth = FindObjectOfType<PlayerHealth>(); 
        } if (healthBar == null) {
            healthBar = FindObjectOfType<HealthBar>(); 
        } if (gameManager == null) {
            gameManager = FindObjectOfType<GameManager>(); // Find GameManager in the scene
        } 

        if (playerHealth == null) {
            Debug.LogWarning("PlayerHealth reference is not assigned.");
        } if (healthBar == null) {
            Debug.LogWarning("HealthBar reference is not assigned.");
        } if (gameManager == null) {
            Debug.LogWarning("GameManager reference is not assigned.");
        } 
    
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        // check if collision is with player or not
        if(collision.gameObject.tag == "Player"){
            Debug.Log("Player collided with health buff.");
            
            // check to see whether references are empty or not
            if (playerHealth != null && healthBar != null) { 
                // add to health bar to get the health buff effect 
                playerHealth.currentHealth += amount; 
                healthBar.SetHealth(playerHealth.currentHealth); 
            } else {
                Debug.LogError("PlayerHealth or HealthBar reference is null.");
            }

            Destroy(gameObject); // destroyes the health buff
            // tells the game manager that a prefab has been destroyed and to spawn another  
            gameManager.PrefabDestroyed(); 
           
        }
    }

    /*
    using UnityEngine;

public class HealthBuff : MonoBehaviour
{
    public int amount; 
    public HealthBar healthBar; 
    public PlayerHealth playerHealth; 
    public GameManager gameManager; 
    public GameManager.SpawnableObject spawnableObject;

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
        if (gameManager == null)
        {
            Debug.LogWarning("GameManager reference is not assigned.");
        }

        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with health buff.");

            if (playerHealth != null && healthBar != null)
            {
                playerHealth.currentHealth += amount;
                healthBar.SetHealth(playerHealth.currentHealth);
            }
            else
            {
                Debug.LogError("PlayerHealth or HealthBar reference is null.");
            }

            Destroy(gameObject);
            gameManager.PrefabDestroyed(spawnableObject);
        }
    }
}

In the `HealthBuff` script, the `spawnableObject` field represents the data associated with the prefab 
of the health buff object that's being spawned. This field should be assigned in the Inspector with the
 appropriate `SpawnableObject` data from the `GameManager`. 

Here's what goes into the `spawnableObject` field in the Inspector:

1. **Prefab**: Assign the health buff prefab GameObject to this field. This is the prefab that will be
 instantiated when a health buff is spawned.

2. **Initial Count**: Set the initial count of the health buff prefab that should be maintained and 
spawned at the beginning of the game or level.

3. **Current Count**: This value will be updated during runtime and doesn't need to be set in the Inspector. 
It represents the current count of spawned health buff prefabs.

So, when you assign a `SpawnableObject` to the `spawnableObject` field in the `HealthBuff` Inspector, 
you're essentially specifying which prefab should be spawned when the health buff is picked up by the player.

    */


}



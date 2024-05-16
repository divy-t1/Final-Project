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
    [SerializeField] private GameManager gameManager;
    private GameManager.SpawnableObject spawnableObject;
    
    
   
 
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

            // tells the game manager that a prefab has been destroyed and to spawn another  
            if (gameManager != null && spawnableObject != null)
            {
                gameManager.PrefabDestroyed(spawnableObject, gameObject); 
            } 

            Destroy(gameObject); // destroys the health buff
           
        }  
    }

    public void Initialize(GameManager gameManager, GameManager.SpawnableObject spawnableObject)
    {
        this.gameManager = gameManager;
        this.spawnableObject = spawnableObject;
    }

}



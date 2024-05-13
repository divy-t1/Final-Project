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
    public SpawningScript spawningScript; 
    //reference to a spawning script that must be attatched to same object
    
    //getting the game objects collider2d and sprite renderes in order to disable later 
    new Collider2D collider2D; 
    private SpriteRenderer spriteRenderer; 
 
    void Start()
    { // checks whether any reference in the game editor is null
        if (playerHealth == null) {
            Debug.LogWarning("PlayerHealth reference is not assigned.");
        } if (healthBar == null) {
            Debug.LogWarning("HealthBar reference is not assigned.");
        } if (spawningScript == null) {
            Debug.LogWarning("SpawningScript reference is not assigned.");
        } 

        collider2D = GetComponent<Collider2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        
        if(collision.gameObject.tag == "Player"){
            Debug.Log("Player collided with health buff.");
            if (playerHealth != null && healthBar != null) { //check to see whether references are empty or not
                playerHealth.currentHealth += amount; 
                healthBar.SetHealth(playerHealth.currentHealth); 
            } else {
                Debug.LogError("PlayerHealth or HealthBar reference is null.");
            }

            collider2D.enabled = false; 
            spriteRenderer.enabled = false; 

            // Invoke the Spawn method after a delay
            //Debug.Log("Invoking Spawn method with a delay of " + spawningScript.delay);
            Invoke("Spawn", spawningScript.delay);
           
        }
    }

    private void Spawn() {
        Debug.Log("Invoking Spawn method with a delay of " + spawningScript.delay);
        spawningScript.Spawn(); 
        collider2D.enabled = true; 
        spriteRenderer.enabled = true; 
        Destroy(gameObject); //destroys the game object after
    }


}



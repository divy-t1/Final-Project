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
    public SpawningScript spawningScript; 

    //create the references for the game objects collider2d and spriterenderer 
    new Collider2D collider2D; 
    private SpriteRenderer spriteRenderer; 

    void Start()
    {
        if (playerHealth == null) {
            Debug.LogWarning("PlayerHealth reference is not assigned.");
        } if (healthBar == null) {
            Debug.LogWarning("HealthBar reference is not assigned.");
        }
        
        //connecting the collider and sprite renderer to the components of the game objects 
        collider2D = GetComponent<Collider2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        //checking if the collision is with the player and will only then proceed with the code to change health
        if(collision.gameObject.tag == "Player"){
            Debug.Log("Player collided with a " + gameObject.name);
            if (playerHealth != null && healthBar != null) {
                playerHealth.currentHealth -= damage; 
                healthBar.SetHealth(playerHealth.currentHealth); 
                collision.transform.position = new Vector3(1.55f, -0.22f, 0); 
            } else {
                Debug.LogError("PlayerHealth or HealthBar reference is null.");
            }

            //turning off the collider and sprite renderer to get the spawn to work 
            collider2D.enabled = false; 
            spriteRenderer.enabled = false; 

            // Invoke the Spawn method after a delay
            //Debug.Log("Invoking Spawn method with a delay of " + spawningScript.delay);
            Invoke("Spawn", spawningScript.delay);
        }
    }

    private void Spawn()
    {
        Debug.Log("Invoking Spawn method with a delay of " + spawningScript.delay);
        spawningScript.Spawn(); 

        Destroy(gameObject); //destroys the game object after
    }
}

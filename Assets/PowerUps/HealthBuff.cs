using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

//adding "IMazeObject" to the class line shows that the public methods and variables 
//part of the interface are exposed to the healthbuff class
public class HealthBuff : MonoBehaviour, IMazeObject
{
    public int amount; 
    //the amount that the health bar has added
    public HealthBar healthBar; 
    //reference to the healthbar that changes with teh collision
    public PlayerHealth playerHealth; 
    //reference to the player health system of the player it collides with
    private GameManager m_GameManager;
    //a local private variable that stores a reference to the game manager class 
    public GameManager gameManager { set => m_GameManager = value; }
    //This is an interface by which other classes can interact with the GameManager 
    //reference that this class stores.
    //The "set =>" syntax is an inline way to define what happens when someone
    //tries to set the value of gameManager. In this case, we simply want to
    //set our private m_GameManager variable equal to the value that they passed in.
    //Since there is no "get" defined, no outside class is allowed to read the value
    //of gameManager.
    private int m_ObjectIndex;
    //This is the private variable where this class stores the index which identifies
    //the "type" of this object. This is assigned by the GameManager.
    public int ObjectIndex { get => m_ObjectIndex; set => m_ObjectIndex = value; }
    //This is the interface by which other classes can interact with our m_ObjectIndex
    //variable. When they try to "get" the variable by reading it, this class will simply
    //return the value of m_ObjectIndex. When they try to "set" the variable by writing it,
    //this class will just set m_ObjectIndex equal to the value passed in.
    
    
   
 
    void Start()
    { // checks whether any reference in the game editor is null
        if (playerHealth == null) {
            playerHealth = FindObjectOfType<PlayerHealth>(); 
        } if (healthBar == null) {
            healthBar = FindObjectOfType<HealthBar>(); 
        }

        if (playerHealth == null) {
            Debug.LogWarning("PlayerHealth reference is not assigned.");
        } if (healthBar == null) {
            Debug.LogWarning("HealthBar reference is not assigned.");
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

            Destroy(gameObject); // destroys the health buff

            // tells the game manager that a prefab has been destroyed and to spawn another  
            // We must pass the object index back to the GameManager so that it knows what
            // type of object was destroyed.
            m_GameManager.PrefabDestroyed(m_ObjectIndex, transform.position);
           
        }  
    }

    

}



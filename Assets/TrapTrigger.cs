using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TrapTrigger : MonoBehaviour, IMazeObject
{
    // Start is called before the first frame update
    public int damage;  
    public HealthBar healthBar; 
    public PlayerHealth playerHealth; 
    private GameManager m_GameManager;
    // the variable that this class interacts with for the reference of game manager 
    public GameManager gameManager { set => m_GameManager = value; }
    // the variable for the interface reference through which other classes can interact with
    private int m_ObjectIndex;
    // the variable through which this class stores the index of the prefab, assinged by game manager
    public int ObjectIndex { get => m_ObjectIndex; set => m_ObjectIndex = value; }
    // the variable through which other classes can interact with our m_objectIndex

    //create the references for the game objects collider2d and spriterenderer 
    

    void Start()
    {
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

            Destroy(gameObject); 
            

            // Invoke the Spawn method after a delay
            //Debug.Log("Invoking Spawn method with a delay of " + spawningScript.delay);
            m_GameManager.PrefabDestroyed(m_ObjectIndex);
        }
    }


}

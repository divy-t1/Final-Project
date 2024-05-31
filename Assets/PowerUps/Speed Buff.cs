using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SpeedBuff : MonoBehaviour, IMazeObject
{
    public float speedIncrease;  // Amount by which the player's speed is increased
    public float duration;  // Duration of the speed buff
    private GameManager m_GameManager;  // Reference to the GameManager
    public GameManager gameManager { set => m_GameManager = value; }
    private int m_ObjectIndex;  // Index to identify the type of this object
    public int ObjectIndex { get => m_ObjectIndex; set => m_ObjectIndex = value; }

    void Start()
    {
        // Optionally, add any initialization code here
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("SpeedBuff OnTriggerEnter2D called.");
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with speed buff.");

            // Get the MovementScript component from the player
            MovementScript movementScript = collision.gameObject.GetComponent<MovementScript>();

            // Apply the speed buff if the MovementScript component is found
            if (movementScript != null)
            {
                Debug.Log("Applying speed buff: " + speedIncrease + " for " + duration + " seconds.");
                movementScript.StartSpeedBuff(speedIncrease, duration);
            }
            else
            {
                Debug.LogError("MovementScript component is not found.");
            }

            Destroy(gameObject);  // Destroy the speed buff

            // Notify the GameManager that a prefab has been destroyed and to spawn another
            m_GameManager.PrefabDestroyed(m_ObjectIndex, transform.position);
        }
        else
        {
            Debug.Log("Collided with non-player object: " + collision.gameObject.name);
        }
    }
    
}

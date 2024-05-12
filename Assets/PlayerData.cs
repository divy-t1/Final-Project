using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int health; 
    public float[] position; 


    public PlayerData(PlayerHealth playerHealth) {//a contructor that acts as the setup function for this class
        health = playerHealth.currentHealth; 

        position = new float[3];
        position[0] = playerHealth.transform.position.x; 
        position[1] = playerHealth.transform.position.y; 
        position[2] = playerHealth.transform.position.z; 
    }
    // Start is called before the first frame update
    
}

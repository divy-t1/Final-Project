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

/*
[System.Serializable]
public class PlayerData
{
    public int health;
    public float[] position;

    public PlayerData(int health, Vector3 position)
    {
        this.health = health;
        this.position = new float[3];
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;
    }
}

*/

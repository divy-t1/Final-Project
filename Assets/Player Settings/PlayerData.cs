using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
// Makes overall class a public reference to other classes
public class PlayerData
{
    public int health; 
    public float[] position;
    public int score; 

    // Constructor sets it so that the player data in arguements is set to the references to call in other classes
    public PlayerData(int health, float[] position, int score)
    {
        this.health = health;
        this.position = position;
        this.score = score; 
    }
}



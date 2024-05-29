using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int health;
    public float[] position;

    public PlayerData(int health, float[] position)
    {
        this.health = health;
        this.position = position;
    }
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

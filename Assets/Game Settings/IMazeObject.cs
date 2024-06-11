using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMazeObject
{
    public GameManager gameManager {set;}
    public int ObjectIndex {get; set; }
}

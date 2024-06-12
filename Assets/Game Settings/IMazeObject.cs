using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface to control the accessability of game manager to ensure no other code in other classes accidently changes it 
public interface IMazeObject 
{
    public GameManager gameManager {set;}
    // Making gameManager a read only variable ensures that when accessing it from elsewhere, it can only be used and not wrote over
    public int ObjectIndex {get; set; }
    // ObjectIndex is just to have better control, it can be manipulated and accessed from other classes as well 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SpeedBuff : MonoBehaviour, IMazeObject
{
    public float speedIncrease; 
    //the amount the speed increases by 
    public float time; 
    //the duration the buff lasts for
    public movementScript movementScript; 
    //reference to the healthbar that changes with teh collision
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
        if (movementScript == null) {
            movementScript = FindObjectOfType<movementScript>(); 
        }
        
        if (movementScript == null) {
            Debug.LogWarning("Movement Script reference is not assigned.");
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            movementScript = collision.gameObject.GetComponent<movementScript>();

            if (movementScript != null) {
                StartCoroutine(movementScript.ApplySpeedBuff(speedIncrease, time));
            }

            Destroy(gameObject);  // Destroy the speed buff object after collision 

            // tells the game manager that a prefab has been destroyed and to spawn another  
            // We must pass the object index back to the GameManager so that it knows what
            // type of object was destroyed.
            m_GameManager.PrefabDestroyed(m_ObjectIndex, transform.position);
        }
    }
}

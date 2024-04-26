using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class mazeWallScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D charRB; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.name == "Maze Walls") {
            charRB.velocity = Vector3.zero; 
            
        }
        
    }
}

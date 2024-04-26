using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class movementScript : MonoBehaviour
{
    // Start is called before the first frame update


    float speed = 1.0F;  
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0F); 
        transform.position += move * speed * Time.deltaTime; 
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class movementScript : MonoBehaviour
{
    // Start is called before the first frame update


    public float speed = 5.0F;
    public float originalSpeed; 

    void Start() {
        originalSpeed = speed; 
    }
    
    // Update is called once per frame
    void Update()
    {

        float moveX = 0f; 
        float moveY = 0f; 

        // Check for arrow inputs of arrow keys 
        if (Input.GetKey(KeyCode.UpArrow)) {
            moveY = 1f; 
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            moveY = -1f; 
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            moveX = 1f; 
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            moveX = -1f; 
        }

        // Apply the movements 
        UnityEngine.Vector3 move = new UnityEngine.Vector3(moveX, moveY, 0f).normalized * speed * Time.deltaTime; 
        transform.position += move; 

    }

    public IEnumerator ApplySpeedBuff(float speedIncrease, float duration)
    {
        speed += speedIncrease;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
    }

}

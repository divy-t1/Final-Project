using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float speed = 5f;  // Movement speed of the player
    private Rigidbody2D rb2D;  // Reference to the player's Rigidbody2D component
    private UnityEngine.Vector2 movement;  // Stores the player's movement input
    private float originalspeed;  // To store the original movement speed

    void Awake()
    {
        // Get the Rigidbody2D component attached to the player
        rb2D = GetComponent<Rigidbody2D>();
        originalspeed = speed;  // Save the original move speed
    }

    void Update()
    {
        // Get horizontal and vertical input (arrow keys or WASD keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Calculate the new position of the player based on the input and movement speed
        UnityEngine.Vector2 newPosition = rb2D.position + movement * speed * Time.fixedDeltaTime;

        // Move the player's Rigidbody2D to the new position using MovePosition
        rb2D.MovePosition(newPosition);
    }

    public void StartSpeedBuff(float speedIncrease, float duration)
    {
        StartCoroutine(SpeedBuffCoroutine(speedIncrease, duration));
    }

    private IEnumerator SpeedBuffCoroutine(float speedIncrease, float duration)
    {
        speed += speedIncrease;  // Increase the move speed
        yield return new WaitForSeconds(duration);  // Wait for the duration of the buff
        speed = originalspeed;  // Reset the move speed to the original value
    }

   
}

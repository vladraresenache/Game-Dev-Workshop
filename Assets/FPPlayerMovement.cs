using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 12f;
    public float gravity = -9.81f; // Strength of gravity

    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        // Check if the player is grounded
        isGrounded = characterController.isGrounded;

        // If the player is grounded and the vertical velocity is negative, reset it to a small negative value
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get horizontal and vertical input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate movement vector based on input
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the player horizontally
        characterController.Move(move * speed * Time.deltaTime);

        // Apply gravity to the vertical velocity
        velocity.y += gravity * Time.deltaTime;

        // Move the player based on the vertical velocity (gravity)
        characterController.Move(velocity * Time.deltaTime);
    }
}

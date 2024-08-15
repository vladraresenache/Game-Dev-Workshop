using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float walkSpeed = 5f;          // Adjusted default speed
    public float walkBackSpeed = 3f;      // Adjusted default speed
    public float rotateSpeed = 700f;      // Adjusted for faster turning
    public float mouseSensitivity = 2f;  // Adjust sensitivity for your needs
    private bool walking = false;
    public Transform playerTrans;
    public Transform cameraTrans; // Reference to the camera attached to the player

    // Smooth movement variables
    private Vector2 smoothMouse;
    private Vector2 mouseSmoothVelocity;

    // Sensitivity for smoothing
    public float smoothTime = 0.1f;

    void Start()
    {
        // Hide the cursor and lock it when the game starts
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void Update()
    {
        HandleRotation();
        HandleMouseLook();
        HandleAnimation();
    }

    void HandleMovement()
    {
        // Forward and backward movement
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection = transform.forward * walkSpeed;
            playerAnim.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection = -transform.forward * walkBackSpeed;
            playerAnim.SetBool("isWalkingBack", true);
        }
        else
        {
            playerAnim.SetBool("isWalking", false);
            playerAnim.SetBool("isWalkingBack", false);
        }

        // Apply movement using AddForce for smoother motion
        playerRigid.AddForce(moveDirection - playerRigid.velocity, ForceMode.VelocityChange);
    }

    void HandleRotation()
    {
        // Handle player rotation using mouse movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            // Rotating the player based on mouse input
            float horizontalInput = Input.GetAxis("Mouse X");
            playerTrans.Rotate(0, horizontalInput * rotateSpeed * Time.deltaTime, 0);
        }
    }

    void HandleMouseLook()
    {
        // Get raw mouse input
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Smooth the mouse input
        smoothMouse.x = Mathf.SmoothDamp(smoothMouse.x, mouseX, ref mouseSmoothVelocity.x, smoothTime);
        smoothMouse.y = Mathf.SmoothDamp(smoothMouse.y, mouseY, ref mouseSmoothVelocity.y, smoothTime);

        // Apply sensitivity and delta time
        float mouseXSmoothed = smoothMouse.x * mouseSensitivity;
        float mouseYSmoothed = smoothMouse.y * mouseSensitivity;

        // Rotate the player around the Y-axis with smoothed mouse X movement
        playerTrans.Rotate(0, mouseXSmoothed, 0);

        // Rotate the camera vertically with smoothed mouse Y movement
        cameraTrans.Rotate(-mouseYSmoothed, 0, 0);

        // Clamp vertical rotation to prevent over-rotation
        Vector3 clampedRotation = cameraTrans.localEulerAngles;
        clampedRotation.x = Mathf.Clamp(clampedRotation.x, -60f, 60f);
        cameraTrans.localEulerAngles = clampedRotation;
    }

    void HandleAnimation()
    {
        // Handle walking forward
        if (Input.GetKeyDown(KeyCode.W))
        {
            walking = true;
            playerAnim.SetTrigger("walk");
            playerAnim.ResetTrigger("idle");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            walking = false;
            playerAnim.ResetTrigger("walk");
            playerAnim.SetTrigger("idle");
        }

        // Handle walking backward
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnim.SetTrigger("walkBack");
            playerAnim.ResetTrigger("idle");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.ResetTrigger("walkBack");
            playerAnim.SetTrigger("idle");
        }
    }

    // Call this method to reset the cursor state when needed
    public void ResetCursorState(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}

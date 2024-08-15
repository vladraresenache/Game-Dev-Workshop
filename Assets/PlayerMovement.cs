using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float walkSpeed, walkBackSpeed, rotateSpeed, mouseSensitivity;
    private bool walking = false;
    public Transform playerTrans;
    public Transform cameraTrans; // Reference to the camera attached to the player

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
        if (Input.GetKey(KeyCode.W))
        {
            playerRigid.velocity = transform.forward * walkSpeed * Time.deltaTime;
            playerAnim.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerRigid.velocity = -transform.forward * walkBackSpeed * Time.deltaTime;
            playerAnim.SetBool("isWalkingBack", true);
        }
        else
        {
            playerRigid.velocity = Vector3.zero;
            playerAnim.SetBool("isWalking", false);
            playerAnim.SetBool("isWalkingBack", false);
        }
    }

    void HandleRotation()
    {
        // A and D key for player rotation
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }

    void HandleMouseLook()
    {
        // Mouse movement for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player around the Y-axis with mouse X movement
        playerTrans.Rotate(0, mouseX, 0);

        // Rotate the camera vertically with mouse Y movement
        cameraTrans.Rotate(-mouseY, 0, 0);

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
}

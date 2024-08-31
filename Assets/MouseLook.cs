using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    public Transform playerBody;
    private float xRotation = 0f;

    public GameObject player;

    public float pickUpRange = 10f;
    public float rotationSensitivity = 100f;
    public float objectRotationSensitivity = 1000f;

    private bool canMoveCamera = true;
    private bool isGamePaused = false;

    private GameObject currentlyHeldObject = null;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public GameObject crosshairCanvas;
    public GameObject inspectCanvas;
    public GameObject exitInspectCanvas;
    public GameObject inventoryCanvas;
    public GameObject pauseMenuCanvas;

    public AssetDisplayManager assetDisplayManager;
    public NotificationManager notificationManager;

    public AudioSource unlockSound;  // AudioSource for the unlock sound effect

    // Zoom variables
    public Camera playerCamera;
    public float zoomFOV = 20f;        // Field of view when zoomed in
    public float normalFOV = 60f;      // Default field of view
    public float zoomSpeed = 10f;      // Speed of zooming in and out

    private bool isZoomed = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inspectCanvas.SetActive(false);
        exitInspectCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false); // Hide pause menu initially

        // Ensure the camera has a reference
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Set the default FOV
        playerCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        bool isInventoryOpen = inventoryCanvas.activeSelf;
        bool isPauseMenuActive = pauseMenuCanvas.activeSelf;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuCanvas.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isInventoryOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }

        if (canMoveCamera && !isInventoryOpen)
        {
            HandleMouseLook();
        }
        else if (currentlyHeldObject != null && !isInventoryOpen && !isGamePaused)
        {
            RotateHeldObject();
            exitInspectCanvas.SetActive(true);
        }

        CheckForInspectableObject();

        if (Input.GetMouseButtonDown(0) && !isInventoryOpen && !isGamePaused)
        {
            if (currentlyHeldObject == null)
            {
                TryPickUpObject();
            }
            else
            {
                DropObject();
            }
        }

        if (Input.GetMouseButtonDown(1) && currentlyHeldObject != null && !isInventoryOpen && !isGamePaused)
        {
            DropObject();
        }

        // Zoom functionality
        if (Input.GetMouseButtonDown(1))  // Right mouse button for zooming
        {
            ToggleZoom();
        }
    }

    void HandleMouseLook()
    {
        // Check if the camera has the tag "MainCamera"
        if (playerCamera.CompareTag("MainCamera"))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
        else
        {
            // Debugging information
           
        }
    }

    void PauseGame()
    {
        isGamePaused = true;
        pauseMenuCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<CharacterController>().enabled = false;
        canMoveCamera = false;
        Time.timeScale = 0f; // Freeze the game

        // Disable other UI elements
        if (inventoryCanvas != null) inventoryCanvas.SetActive(false);
    }

    void ResumeGame()
    {
        isGamePaused = false;
        pauseMenuCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<CharacterController>().enabled = true;
        canMoveCamera = true;
        Time.timeScale = 1f; // Unfreeze the game
    }

    public void OpenInventory()
    {
        inventoryCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<CharacterController>().enabled = false;
        canMoveCamera = false;
        Time.timeScale = 0f; // Freeze the game while inventory is open
    }

    public void CloseInventory()
    {
        inventoryCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<CharacterController>().enabled = true;
        canMoveCamera = true;
        Time.timeScale = 1f; // Unfreeze the game when inventory is closed
    }

    void CheckForInspectableObject()
    {
        if (Camera.main != null && currentlyHeldObject == null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, pickUpRange))
            {
                GameObject gameObj = hit.collider.gameObject;

                if (gameObj.CompareTag("canPickUp") || gameObj.CompareTag("readable"))
                {
                    inspectCanvas.SetActive(true);
                }
                else
                {
                    inspectCanvas.SetActive(false);
                }
            }
            else
            {
                inspectCanvas.SetActive(false);
            }
        }
    }

    void TryPickUpObject()
    {
        if (Camera.main != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject gameObj = hit.collider.gameObject;

                if (gameObj.CompareTag("canPickUp"))
                {
                    PickUpObject(gameObj);
                }
                else if (gameObj.CompareTag("readable"))
                {
                    ReadObject(gameObj);
                }
            }
            else
            {
                Debug.LogError("Main camera is not found. Please ensure that a camera is tagged with 'MainCamera'.");
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            currentlyHeldObject = pickUpObj;
            originalPosition = pickUpObj.transform.position;
            originalRotation = pickUpObj.transform.rotation;

            canMoveCamera = false;
            crosshairCanvas.SetActive(false);
            inspectCanvas.SetActive(false);
            exitInspectCanvas.SetActive(true);
            pickUpObj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.7f;
            player.GetComponent<CharacterController>().enabled = false;

            Physics.IgnoreCollision(pickUpObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }

    void DropObject()
    {
        if (currentlyHeldObject != null)
        {
            currentlyHeldObject.transform.position = originalPosition;
            currentlyHeldObject.transform.rotation = originalRotation;

            canMoveCamera = true;
            player.GetComponent<CharacterController>().enabled = true;
            crosshairCanvas.SetActive(true);
            exitInspectCanvas.SetActive(false);

            Physics.IgnoreCollision(currentlyHeldObject.GetComponent<Collider>(), player.GetComponent<Collider>(), false);

            currentlyHeldObject = null;
        }
    }

    void RotateHeldObject()
    {
        float mouseX = Input.GetAxis("Mouse X") * objectRotationSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * objectRotationSensitivity * Time.deltaTime;

        currentlyHeldObject.transform.Rotate(Vector3.up, -mouseX, Space.World);
        currentlyHeldObject.transform.Rotate(Vector3.right, mouseY, Space.World);
    }

    void ReadObject(GameObject readableObj)
    {
        string entryIdentifier = readableObj.name;

        if (assetDisplayManager != null)
        {
            int textIndex = GetTextIndexFromIdentifier(entryIdentifier);
            if (textIndex != -1)
            {
                if (!assetDisplayManager.IsTextUnlocked(textIndex))
                {
                    assetDisplayManager.UnlockText(textIndex);

                    // Play the unlock sound effect
                    if (unlockSound != null)
                    {
                        unlockSound.Play();
                    }

                    if (notificationManager != null)
                    {
                        notificationManager.ShowNotification("New Document Unlocked\nPress [Tab] to read");
                    }
                }
            }
        }
    }

    int GetTextIndexFromIdentifier(string identifier)
    {
        switch (identifier)
        {
            case "Clue1":
                return 0;
            case "Clue2":
                return 1;
            case "Clue3":
                return 2;
            case "Clue4":
                return 3;
            case "Clue5":
                return 4;
            case "Clue6":
                return 5;
            default:
                return -1;
        }
    }

    void ToggleZoom()
    {
        if (isZoomed)
        {
            playerCamera.fieldOfView = normalFOV;
        }
        else
        {
            playerCamera.fieldOfView = zoomFOV;
        }
        isZoomed = !isZoomed;
    }
}
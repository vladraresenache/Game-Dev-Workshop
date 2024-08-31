using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneOnHold : MonoBehaviour
{
    public float holdDuration = 5f; // Duration to hold the key to trigger the scene change

    private float holdTime = 0f;
    private bool isHolding = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return)) // Check if Enter key (Return) is being held
        {
            if (!isHolding)
            {
                // Start holding the key
                isHolding = true;
                holdTime = 0f;
            }
            else
            {
                // Update the hold time
                holdTime += Time.deltaTime;

                // Check if the key has been held for the required duration
                if (holdTime >= holdDuration)
                {
                    LoadNextScene();
                }
            }
        }
        else
        {
            // Reset hold state if the key is released
            isHolding = false;
            holdTime = 0f;
        }
    }

    private void LoadNextScene()
    {
        // Get the current scene index and load the next one
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is valid
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("There is no next scene. Please add more scenes to the build settings.");
        }
    }
}

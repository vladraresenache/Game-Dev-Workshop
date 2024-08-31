using UnityEngine;
using System.Collections;

public class TriggerDoor2 : MonoBehaviour
{
    public Animator doorAnimator;                // Reference to the Animator controlling the door
    public string doorCloseTriggerName = "isOpen"; // Name of the Animator parameter that controls the door's state
    public AudioSource closeSound;              // Reference to the AudioSource for the closing sound
    private bool hasPassed = false;
    public float delay=2f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is the one triggering the event
        if (other.CompareTag("Player"))
        {
            if (hasPassed)
            {
                // Start the coroutine to close the door with a delay
                StartCoroutine(CloseDoorWithDelay(delay)); // 1 second delay
                hasPassed = false;
            }
            else
            {
                hasPassed = true;
            }
        }
    }

    private IEnumerator CloseDoorWithDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Close the door by setting the animator parameter to false
        doorAnimator.SetBool(doorCloseTriggerName, false);

        // Play the closing sound if assigned
        if (closeSound != null)
        {
            closeSound.Play();
        }

        Debug.Log("Player passed through the trigger. Closing the door after delay.");
    }
}

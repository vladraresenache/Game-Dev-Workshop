using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    public Animator doorAnimator;  // Reference to the Animator controlling the door
    public string doorCloseTriggerName = "isOpen";  // Name of the Animator parameter that controls the door's state

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is the one triggering the event
        if (other.CompareTag("Player"))
        {
            // Close the door by setting the animator parameter to false
            doorAnimator.SetBool(doorCloseTriggerName, false);
            Debug.Log("Player passed through the trigger. Closing the door.");
        }
    }
}

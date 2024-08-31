using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TriggerAnimation : MonoBehaviour
{
    public Animator animator;                 // Animator component controlling the animation
    public Transform playerTransform;         // Reference to the player's transform
    public TextMeshProUGUI proximityText;     // Optional: Text to display when the player is in range
    public Vector3 originalPosition;          // Store the object's original position
    public AudioSource audioSource;           // AudioSource component for playing the sound
    public AudioClip triggerSound;            // The sound to play when the animation is triggered
    public float soundDelay = 1f;             // Time to wait before playing the sound
    private bool isAnimationTriggered = false; // To check if the animation has already been triggered
    private float activationDistance = 2f;    // Distance within which the player can trigger the animation

    void Start()
    {
        // Save the original position of the object
        originalPosition = transform.position;

        // Ensure proximity text is deactivated at the start
        if (proximityText != null)
        {
            proximityText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isAnimationTriggered)
        {
            // Check if the animation has finished
            if (IsAnimationFinished())
            {
                ResetObject();
            }
            return;
        }

        // Calculate the distance between the player and the object
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        // Show proximity text when the player is within range
        if (distanceToPlayer <= activationDistance)
        {
            
            if (proximityText != null)
            {
                proximityText.gameObject.SetActive(true);
            }

            // Trigger the animation and sound when the player presses "E"
            if (Input.GetKeyDown(KeyCode.E))
            {
                TrigAnimation();
                StartCoroutine(PlaySoundWithDelay());  // Start the coroutine to play sound with delay
            }
        }
        else
        {
            // Hide proximity text when the player is out of range
            if (proximityText != null)
            {
                proximityText.gameObject.SetActive(false);
            }
        }
    }

    void TrigAnimation()
    {
        // Trigger the animation
        animator.SetTrigger("PlayAnimation");
        isAnimationTriggered = true;  // Prevent the animation from being triggered multiple times

        // Hide the proximity text
        if (proximityText != null)
        {
            proximityText.gameObject.SetActive(false);
        }

        Debug.Log("Animation triggered.");
    }

    IEnumerator PlaySoundWithDelay()
    {
        // Wait for the specified delay before playing the sound
        yield return new WaitForSeconds(soundDelay);

        // Play the sound if the AudioSource and AudioClip are set
        if (audioSource != null && triggerSound != null)
        {
            audioSource.PlayOneShot(triggerSound);
            Debug.Log("Sound played after delay.");
        }
    }

    bool IsAnimationFinished()
    {
        // Check if the current state is not playing any animation (i.e., it's in the idle state or finished state)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1 && !animator.IsInTransition(0);
    }

    void ResetObject()
    {
        // Reset the object's position to the original position
        transform.position = originalPosition;

        // Reset the animation state
        isAnimationTriggered = false;

        Debug.Log("Object reset and ready for re-trigger.");
    }
}

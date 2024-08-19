using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isWalkingBackHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isWalkingBackHash = Animator.StringToHash("isWalkingBack");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isWalkingBack = animator.GetBool(isWalkingBackHash);

        bool forwardPress = Input.GetKey("w");
        bool backwardPress = Input.GetKey("s");

        // Walking Forward
        if (!isWalking && forwardPress)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (isWalking && !forwardPress)
        {
            animator.SetBool(isWalkingHash, false);
        }

        // Walking Backward
        if (!isWalkingBack && backwardPress)
        {
            animator.SetBool(isWalkingBackHash, true);
        }
        if (isWalkingBack && !backwardPress)
        {
            animator.SetBool(isWalkingBackHash, false);
        }
    }
}

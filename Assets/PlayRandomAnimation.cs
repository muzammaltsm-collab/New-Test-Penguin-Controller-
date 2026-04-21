using System.Collections;
using UnityEngine;

public class PlayRandomAnimation : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    // Name of the idle animation
    public string idleAnimationName = "Idle";

    // Minimum and maximum time to wait before playing the idle animation
    [SerializeField] float minWaitTime = 0.1f;
    [SerializeField] float maxWaitTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        // Start the coroutine to play the idle animation at random intervals
        StartCoroutine(PlayIdleAnimationAtRandomIntervals());
    }

    // Coroutine to play the idle animation at random intervals
    IEnumerator PlayIdleAnimationAtRandomIntervals()
    {

        // Wait for a random number of seconds
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        // Play the idle animation
        animator.Play(idleAnimationName);

    }
}

using UnityEngine;

public class TimelineMovementAnimator : MonoBehaviour
{
    private Animator animator;
    public Vector3 lastPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        float deltaX = currentPosition.x - lastPosition.x;

        if (deltaX > 0.01f)
        {
            // Moving right
            animator.Play("RunRight");
        }
        else if (deltaX < -0.01f)
        {
            // Moving left
            animator.Play("RunLeft");
        }

        // Update the last position
        lastPosition = currentPosition;
    }
}

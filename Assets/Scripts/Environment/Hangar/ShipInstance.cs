using UnityEngine;

public class ShipInstance : MonoBehaviour
{
    public bool IsLaunching;
    public Animator LaunchAnimator;

    private void Awake()
    {
        LaunchAnimator = GetComponent<Animator>();
    }

    public void Launch()
    {
        IsLaunching = true;
        LaunchAnimator.SetBool("Launch", true);
    }

    private void Update()
    {
        if (IsLaunching)
        {
            // If the animation has finished, delete the object
            if (LaunchAnimator.GetCurrentAnimatorStateInfo(0).length >
                LaunchAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                Destroy(gameObject);
            }
        }
    }
}

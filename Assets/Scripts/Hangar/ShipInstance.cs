using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInstance : MonoBehaviour
{
    public bool isLaunching;
    public Animator launchAnimator;

    public void Launch()
    {
        isLaunching = true;
    }

    private void Update()
    {
        if (isLaunching)
        {
            launchAnimator.SetBool("launch", true);
        }

        // If the animation has finished, delete the object
        if(launchAnimator.GetCurrentAnimatorStateInfo(0).length >
            launchAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            Destroy(gameObject);
        }

    }
}

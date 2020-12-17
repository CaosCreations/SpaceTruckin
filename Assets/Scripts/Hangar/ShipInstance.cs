using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInstance : MonoBehaviour
{
    public bool isLaunching;
    public Animator launchAnimator;

    private void Awake()
    {
        launchAnimator = GetComponent<Animator>();
    }

    public void Launch()
    {
        isLaunching = true;
        launchAnimator.SetBool("Launch", true);
    }

    private void Update()
    {
        if (isLaunching)
        {
            // If the animation has finished, delete the object
            if (launchAnimator.GetCurrentAnimatorStateInfo(0).length >
                launchAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                Destroy(gameObject);
            }
        }
    }
}

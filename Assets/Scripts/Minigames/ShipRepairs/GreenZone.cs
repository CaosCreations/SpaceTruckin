using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenZone : MonoBehaviour
{
    private float startingXScale;

    private void Start()
    {
        startingXScale = transform.localScale.x;
    }

    // Call this to increase the difficulty 
    public void ReduceSize()
    {
        transform.localScale += new Vector3(
            -RepairsConstants.SizeDecrease, 0f, 0f); 
    }

    public void ResetSize()
    {
        transform.localScale = new Vector3(
            startingXScale, transform.localScale.y, transform.localScale.z); 
    }
}

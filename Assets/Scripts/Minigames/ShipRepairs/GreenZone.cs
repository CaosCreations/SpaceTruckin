using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenZone : MonoBehaviour
{
    private float startingXScale;
    private float sizeDecrease;

    private void Start()
    {
        startingXScale = transform.localScale.x;
        sizeDecrease = 0.01f; 
    }

    // Call this to increase the difficulty 
    public void DecreaseSize()
    {
        transform.localScale += new Vector3(-sizeDecrease, 0f, 0f); 
    }

    public void ResetSize()
    {
        transform.localScale = new Vector3(
            startingXScale, transform.localScale.y, transform.localScale.z); 
    }
}

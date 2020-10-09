using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour
{
    public GameObject redZone;
    public GameObject greenZone;
    public GameObject indicator; 

    public Vector3 workstationRotation;

    public float rotationSpeed = 16f;
    public float rotationAngle = 8f; 

    private void Start()
    {
        workstationRotation = transform.localEulerAngles;
    }

    public void RotateWorkStation()
    {
        //transform.localEulerAngles += new Vector3(0f, 0f, rotationSpeed);
        //transform.Rotate(Vector3.forward, rotationSpeed);
        transform.Rotate(0f, 0f, rotationSpeed);
    }

    public void ReverseRotationDirection()
    {
        rotationSpeed = rotationSpeed > 0 ? rotationSpeed * -1 : Mathf.Abs(rotationSpeed); 
    }

    public void IncreaseRotationSpeed(float speedIncrease) 
    {
        rotationSpeed += speedIncrease;
    }

    //ncap
    public void DecreaseGreenZoneSize(float sizeDecrease)
    {
        greenZone.transform.localScale -= new Vector3(sizeDecrease, 0f, 0f);
    }

    private void Update()
    {
        RotateWorkStation(); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour
{
    public GameObject redZone;
    public GameObject greenZone; 

    public Vector3 workstationRotation;
        
    public float rotationSpeed = 0.5f; 

    private void Start()
    {
        workstationRotation = transform.localEulerAngles;
    }

    public void RotateWorkStation()
    {
        transform.localEulerAngles += new Vector3(0f, 0f, rotationSpeed);
    }

    private void Update()
    {
        RotateWorkStation(); 
    }
}

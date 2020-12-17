using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Simple handler for office door box collider
public class OfficeDoor : MonoBehaviour
{

    public Vector3 doorOpenOffset;

    private void OnTriggerEnter(Collider other)
    {
        transform.parent.Translate(doorOpenOffset);
        transform.Translate(-doorOpenOffset);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.Translate(-doorOpenOffset);
        transform.Translate(doorOpenOffset);
    }
}

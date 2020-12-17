using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Simple handler for office door box collider
public class OfficeDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.Translate(100, 0, 0);
        transform.Translate(-100, 0, 0);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.Translate(-100, 0, 0);
        transform.Translate(100, 0, 0);
    }
}

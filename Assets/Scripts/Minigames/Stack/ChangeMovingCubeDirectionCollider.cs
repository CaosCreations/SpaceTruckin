using System;
using UnityEngine;

public class ChangeMovingCubeDirectionCollider : MonoBehaviour
{
    public Action CollisionWithMovingCubeEvent;

    private void OnTriggerEnter(Collider other)
    {
        CollisionWithMovingCubeEvent.Invoke();
    }
}

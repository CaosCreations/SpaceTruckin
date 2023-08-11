using System;
using UnityEngine;

public class PositionConstrainer : MonoBehaviour
{
    private BoxCollider rectangleCollider;

    [SerializeField]
    private float offsetDistance = 1f;

    private Action callback;

    public void Init(Action cb)
    {
        callback = cb;
    }

    private void Awake()
    {
        rectangleCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PlayerConstants.PlayerTag))
            return;

        callback?.Invoke();

        Vector3 playerPosition = other.transform.position;
        Vector3 centralPoint = rectangleCollider.bounds.GetCentralPointAlongLongEdge();
        Vector3 newPosition = GetResetPosition(playerPosition, centralPoint);
        other.transform.position = newPosition;
    }

    private Vector3 GetResetPosition(Vector3 playerPosition, Vector3 centralPoint)
    {
        float offsetZ = playerPosition.z - centralPoint.z;
        float offsetX = playerPosition.x - centralPoint.x;

        if (Mathf.Abs(offsetZ) > Mathf.Abs(offsetX))
        {
            if (offsetZ > 0)
            {
                return new Vector3(centralPoint.x, centralPoint.y, centralPoint.z + offsetDistance);
            }
            else
            {
                return new Vector3(centralPoint.x, centralPoint.y, centralPoint.z - offsetDistance);
            }
        }
        else
        {
            if (offsetX > 0)
            {
                return new Vector3(centralPoint.x + offsetDistance, centralPoint.y, centralPoint.z);
            }
            else
            {
                return new Vector3(centralPoint.x - offsetDistance, centralPoint.y, centralPoint.z);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (rectangleCollider == null)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + rectangleCollider.center, rectangleCollider.size);
    }
}

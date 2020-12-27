using UnityEngine;

public class AIDestination : MonoBehaviour
{
    // The x and z values that will determine the next position of the destination
    private int xPos;
    private int zPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            xPos = Random.Range(1, 200);
            zPos = Random.Range(1, 200);
            transform.position = new Vector3(xPos, transform.position.y, zPos);
        }
    }
}

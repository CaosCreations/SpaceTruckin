using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;

    private Rigidbody rb; 
    private CanvasManager canvasManager;

    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();

        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }

        canvasManager = GetComponent<CanvasManager>();

        // Terminal canvas instance scales down 
        // the mission buttons to (0,0) for some reason. 
        ScaleUpMissionButtons(); 
    }

    // Could break this out into a trigger utils class, 
    // as it is likely to be used by other interactable objects. 
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            if (Input.GetKeyDown(PlayerConstants.action))
            {
                canvasManager.ActivateCanvas();
            }
            else if (Input.GetKeyDown(PlayerConstants.exit))
            {
                canvasManager.DeactivateCanvas();
            }
        }
    }

    private void ScaleUpMissionButtons()
    {
        foreach (Transform _transform in gameManager.missionsPanel.transform)
        {
            _transform.localScale = Vector2.one; 
        }
    }
}

using UnityEngine;

public class Battery : MonoBehaviour
{
    public bool IsCharged { get; set; }
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject parentObject;

    private void Start()
    {
        SetColour();
    }

    public void Charge()
    {
        IsCharged = true;
        SetColour();
    }

    public void Discharge()
    {
        IsCharged = false;
        SetColour();
    }

    private void SetColour()
    {
        meshRenderer.material.color = IsCharged ?
            HangarConstants.ChargedBatteryColour :
            HangarConstants.DepletedBatteryColour;
    }

    public bool IsPlayerHolding()
    {
        return gameObject.ObjectWithTagIsParent(PlayerConstants.PlayerTag);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PlayerConstants.PlayerTag)
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            Debug.Log("Battery action");
            parentObject.transform.SetParent(other.transform);
        }
    }
}

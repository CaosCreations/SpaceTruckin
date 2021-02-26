using UnityEngine;

public class LicencesUI : MonoBehaviour
{
    public GameObject treeContainer;
    public GameObject detailsContainer; 
    public GameObject nodePrefab;


    private void PopulateTree()
    {
        foreach (Licence licence in LicencesManager.GetLicencesInTierOrder())
        {
            // Every tier, make a new HLG 

            // Or Make 2D 
        }
    }

    private void HandlePointInvestment(Licence licence)
    {

    }


}

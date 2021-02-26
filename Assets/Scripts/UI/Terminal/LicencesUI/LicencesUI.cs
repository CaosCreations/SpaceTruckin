using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LicencesUI : MonoBehaviour
{
    public GameObject treeContainer;
    public GameObject detailsContainer; 
    public GameObject nodePrefab;


    //TC with VLG
    private void PopulateTree()
    {
        var licenceGroups = LicencesManager.GetLicencesGroupedByTiers();

        for (int i = 0; i < licenceGroups.Count; i++)
        {
            // Init tier 

        }
        
        
        
        foreach (Licence licence in LicencesManager.GetLicencesInTierOrder())
        {
            // Every tier, make a new HLG 

            // Or Make 2D 



        }
    }

    private void InitTier(List<Licence> licences)
    {
        GameObject tier = new GameObject("Tier");
        tier.transform.parent = treeContainer.transform;
        tier.AddComponent<HorizontalLayoutGroup>();

    }

}

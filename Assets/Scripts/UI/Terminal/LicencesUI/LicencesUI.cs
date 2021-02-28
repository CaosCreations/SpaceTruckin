using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LicencesUI : MonoBehaviour
{
    public GameObject treeContainer;
    public GameObject detailsContainer; 
    public GameObject nodePrefab;

    private void Start()
    {
        PopulateTree();
    }

    private void PopulateTree()
    {
        var licenceGroups = LicencesManager.GetLicencesGroupedByTiers();
        
        if (licenceGroups != null)
        {
            for (int i = 1; i <= licenceGroups.Count; i++)
            {
                if (licenceGroups[i] == null)
                {
                    continue;
                }
                InitTier(licenceGroups[i]);
            }
        }
    }

    private void InitTier(List<Licence> licences)
    {
        GameObject tier = new GameObject("Tier");
        tier.transform.parent = treeContainer.transform;
        tier.AddComponent<HorizontalLayoutGroup>();

        foreach (Licence licence in licences)
        {
            if (licence == null)
            {
                continue;
            }
            GameObject node = Instantiate(nodePrefab, tier.transform);
            node.GetComponent<LicenceNode>().Init(licence);
        }
    }
}

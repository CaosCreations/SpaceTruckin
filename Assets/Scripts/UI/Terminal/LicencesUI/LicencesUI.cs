using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LicencesUI : MonoBehaviour
{
    [SerializeField] private GameObject treeContainer;
    [SerializeField] private GameObject detailsContainer;
    [SerializeField] private GameObject tierPrefab;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Text pointsText; 

    private void Start()
    {
        PopulateTree();
        UpdatePointsText();
        LicenceNode.OnLicenceAcquisition += UpdatePointsText;
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
        GameObject tier = Instantiate(tierPrefab, treeContainer.transform);
        foreach (Licence licence in licences)
        {
            if (licence == null)
            {
                continue;
            }
            GameObject node = Instantiate(nodePrefab, tier.transform);
            node.name = $"{licence.Name} node";
            node.GetComponent<LicenceNode>().Init(licence);
        }
    }

    private void UpdatePointsText()
    {
        pointsText.SetText("Licence Points Remaining: " + PlayerManager.Instance.LicencePoints, FontType.Subtitle);
    }
}

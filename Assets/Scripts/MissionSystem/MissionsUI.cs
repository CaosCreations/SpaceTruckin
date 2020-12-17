using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MissionsUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject missionItemPrefab;


    void Start()
    {
        
    }

    private void OnEnable()
    {
        CleanScrollView();
        PopulateScrollView();
    }

    void CleanScrollView()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void PopulateScrollView()
    {
        foreach (Mission mission in MissionsManager.Instance.missionsAcceptedInNoticeBoard)
        {
            GameObject scrollItem = Instantiate(missionItemPrefab, scrollViewContent.transform);
            MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
            missionItem.Init(mission);
        }
    }
}

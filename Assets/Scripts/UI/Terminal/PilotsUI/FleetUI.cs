using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FleetUI : MonoBehaviour
{
    // The parent object 
    public GameObject fleetPanel;

    // Scroll view for pilots currently hired
    public GameObject hiredPilotsList;
    public GameObject hiredPilotsListHeader;
    public Transform hiredPilotsScrollViewContent;

    // Scroll view for pilots available to hire
    public GameObject pilotsForHireList;
    public GameObject pilotsForHireListHeader;
    public Transform pilotsForHireScrollViewContent;

    public GameObject pilotListItemPrefab;
    public GameObject backButtonPrefab;
    public GameObject hireButtonPrefab;
    private Button hireButton;

    private GameObject pilotProfilePanel;
    private Text pilotDetailsText;
    private Image pilotAvatar;
    private Image shipAvatar;

    private void Awake()
    {
        GeneratePilotProfile();
    }

    private void GeneratePilotProfile()
    {
        GeneratePilotProfilePanel();
        GenerateShipAvatar();
        GeneratePilotAvatar();
        GeneratePilotDetails();
        GeneratePilotProfileButton(
            backButtonPrefab, PilotsConstants.BackButtonAnchors, () => SetPilotListVisibility(true));
    }

    private void OnEnable()
    {
        SetPilotListVisibility(true);
        PopulateScrollViews();
    }

    private void SetPilotListVisibility(bool visible)
    {
        hiredPilotsList.SetActive(visible);
        hiredPilotsListHeader.SetActive(visible);
        pilotsForHireList.SetActive(visible);
        pilotsForHireListHeader.SetActive(visible);
        pilotProfilePanel.SetActive(!visible);
    }

    private void PopulateScrollViews()
    {
        PopulateScrollView(PilotsManager.HiredPilots, hiredPilotsScrollViewContent);
        PopulateScrollView(PilotsManager.PilotsForHire, pilotsForHireScrollViewContent);
    }

    private void PopulateScrollView(Pilot[] pilots, Transform scrollViewContent)
    {
        scrollViewContent.DestroyDirectChildren();
        if (pilots != null)
        {
            foreach (Pilot pilot in pilots)
            {
                if (pilot == null)
                {
                    continue;
                }
                Instantiate(pilotListItemPrefab, scrollViewContent)
                    .GetComponent<Button>()
                    .AddOnClick(() => ShowPilotProfilePanel(pilot))
                    .SetText(pilot.Name, FontType.ListItem);
            }
        }
    }

    private void ShowPilotProfilePanel(Pilot pilot)
    {
        SetPilotListVisibility(visible: false);

        shipAvatar.sprite = pilot.Ship.Avatar;
        pilotAvatar.sprite = pilot.Avatar;
        pilotDetailsText.SetText(BuildDetailsString(pilot));

        if (pilot.IsHired)
        {
            hireButton.DestroyIfExists();
            return;
        }

        if (hireButton == null)
        {
            hireButton = GeneratePilotProfileButton(hireButtonPrefab, PilotsConstants.HireButtonAnchors, () => HirePilot(pilot));
        }
        else
        {
            hireButton.AddOnClick(() => HirePilot(pilot));
        }
        hireButton.interactable = PlayerManager.Instance.CanSpendMoney(pilot.HireCost);
    }

    private Button GeneratePilotProfileButton(GameObject prefab, (Vector2, Vector2) anchors, UnityAction callback)
    {
        GameObject newButton = Instantiate(prefab);
        if (pilotProfilePanel != null)
        {
            newButton.transform.SetParent(pilotProfilePanel.transform);
        }

        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.Reset();
        rectTransform.SetAnchors(anchors);

        Button button = newButton.GetComponent<Button>();
        button.AddOnClick(callback);
        return button;
    }

    private void GeneratePilotProfilePanel()
    {
        pilotProfilePanel = new GameObject(PilotsConstants.ProfilePanelName);
        pilotProfilePanel.transform.SetParent(fleetPanel.transform);

        RectTransform rectTransform = pilotProfilePanel.AddComponent<RectTransform>();
        rectTransform.Reset();
        rectTransform.Stretch();

        pilotProfilePanel.SetActive(false);
    }

    private void GenerateShipAvatar()
    {
        GameObject shipAvatarObject = new GameObject().ScaffoldUI(
            PilotsConstants.ShipAvatarObjectName, pilotProfilePanel, PilotsConstants.ShipAvatarAnchors);

        shipAvatar = shipAvatarObject.AddComponent<Image>();
    }

    private void GeneratePilotAvatar()
    {
        GameObject pilotAvatarObject = new GameObject().ScaffoldUI(
            PilotsConstants.PilotAvatarObjectName, pilotProfilePanel, PilotsConstants.PilotAvatarAnchors);

        pilotAvatar = pilotAvatarObject.AddComponent<Image>();

        // Set the width and height of the RectTransform to be the same
        RectTransform rectTransform = pilotAvatarObject.GetComponent<RectTransform>();
        float size = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }

    private void GeneratePilotDetails()
    {
        GameObject pilotDetails = new GameObject().ScaffoldUI(
            PilotsConstants.DetailsObjectName, pilotProfilePanel, PilotsConstants.PilotDetailsAnchors);

        pilotDetails.GetComponent<RectTransform>().SetPadding(Side.Top, PilotsConstants.TopPadding);
        pilotDetailsText = pilotDetails.AddComponent<Text>();
        pilotDetailsText.SetDefaultFont();
        pilotDetailsText.resizeTextForBestFit = true;
        pilotDetailsText.color = Color.black;
    }

    private string BuildDetailsString(Pilot pilot)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLineWithBreaks("Name: " + pilot.Name);
        builder.AppendLineWithBreaks("Ship: " + pilot.Ship.Name);
        builder.AppendLineWithBreaks("Level: " + pilot.Level);
        builder.AppendLineWithBreaks("Experience: " + pilot.CurrentXp);
        builder.AppendLineWithBreaks("Navvy skill level: " + pilot.Navigation);
        builder.AppendLineWithBreaks("Savvy skill level: " + pilot.Savviness);
        builder.AppendLineWithBreaks("Missions completed: " + pilot.MissionsCompleted, 1);
        builder.AppendLineWithBreaks("Likes: " + pilot.Like);
        builder.AppendLineWithBreaks("Dislikes: " + pilot.Dislike, 1);
        builder.AppendLineWithBreaks("Cost to hire: " + pilot.HireCost);
        return builder.ToString();
    }

    private void HirePilot(Pilot pilot)
    {
        if (pilot != null && PlayerManager.Instance.CanSpendMoney(pilot.HireCost))
        {
            PlayerManager.Instance.SpendMoney(pilot.HireCost);
            PilotsManager.Instance.HirePilot(pilot);
            hireButton.interactable = false;
            hireButton.SetText("Pilot Hired!");
            PopulateScrollViews();
        }
    }
}
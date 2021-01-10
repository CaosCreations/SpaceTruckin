using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 

public class PilotsUI : MonoBehaviour
{
	// The parent object 
	public GameObject crewPanel;
	
	/// <summary>
	///		There are two scroll views, one for the list of currently hired pilots,
	///		and the other for the list of pilots that are available to hire. 
	/// </summary>
	public GameObject hiredPilotsList;
	public GameObject hiredPilotsListHeader;
	public Transform hiredPilotsScrollViewContent;
	
	public GameObject pilotsForHireList; 
	public GameObject pilotsForHireListHeader;
	public Transform pilotsForHireScrollViewContent;

	public GameObject crewItemPrefab;
	public GameObject buttonPrefab;
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
			backButtonPrefab, PilotsConstants.backButtonAnchors, () => SetPilotListVisibility(true));
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
		PopulateScrollView(PilotsManager.Instance.GetHiredPilots(), hiredPilotsScrollViewContent);
		PopulateScrollView(PilotsManager.Instance.GetPilotsForHire(), pilotsForHireScrollViewContent);
    }

	private void PopulateScrollView(Pilot[] pilots, Transform scrollViewContent)
	{
		CleanScrollView(scrollViewContent);
		if (pilots != null)
        {
			foreach (Pilot pilot in pilots)
			{
				GameObject crewItem = Instantiate(crewItemPrefab, scrollViewContent);
				crewItem.GetComponent<Button>().AddOnClick(() => ShowPilotProfilePanel(pilot));
				crewItem.GetComponentInChildren<Text>().text = pilot.pilotName;
			}
        }
	}

	private void CleanScrollView(Transform scrollViewContent)
    {
		foreach (Transform transform in scrollViewContent)
        {
			Destroy(transform.gameObject);
        }
    }
	
	private void ShowPilotProfilePanel(Pilot pilot)
	{
		SetPilotListVisibility(visible: false);
		shipAvatar.sprite = pilot.ship.shipAvatar;
		pilotAvatar.sprite = pilot.avatar;
		pilotDetailsText.text = BuildDetailsString(pilot);

		// If the pilot doesn't already work for us, then set up a button to handle hiring him 
		if (!pilot.isHired)
        {
			hireButton = GeneratePilotProfileButton(
				hireButtonPrefab, PilotsConstants.hireButtonAnchors, () => HirePilot(pilot));
		}
		else if (pilot.isHired && hireButton != null)
        {
			Destroy(hireButton.gameObject);
        }
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
		pilotProfilePanel = new GameObject(PilotsConstants.profilePanelName);
		pilotProfilePanel.transform.SetParent(crewPanel.transform);

		RectTransform rectTransform = pilotProfilePanel.AddComponent<RectTransform>();
		rectTransform.Reset();
		rectTransform.Stretch();
			
		pilotProfilePanel.SetActive(false);
	}

	private void GenerateShipAvatar()
	{
		GameObject shipAvatarObject = new GameObject().ScaffoldUI(
			PilotsConstants.shipAvatarObjectName, pilotProfilePanel, PilotsConstants.shipAvatarAnchors);

		shipAvatar = shipAvatarObject.AddComponent<Image>();
	}

	private void GeneratePilotAvatar()
	{
		GameObject pilotAvatarObject = new GameObject().ScaffoldUI(
			PilotsConstants.pilotAvatarObjectName, pilotProfilePanel, PilotsConstants.pilotAvatarAnchors);

		pilotAvatar = pilotAvatarObject.AddComponent<Image>();
	}

	private void GeneratePilotDetails()
    {
		GameObject pilotDetails = new GameObject().ScaffoldUI(
			PilotsConstants.detailsObjectName, pilotProfilePanel, PilotsConstants.pilotDetailsAnchors);

		pilotDetails.GetComponent<RectTransform>().SetPadding(Side.Top, PilotsConstants.topPadding);
		pilotDetailsText = pilotDetails.AddComponent<Text>();
		pilotDetailsText.SetDefaultFont();
		pilotDetailsText.resizeTextForBestFit = true; 
		pilotDetailsText.color = Color.black;
	}

	private string BuildDetailsString(Pilot pilot)
	{
		StringBuilder builder = new StringBuilder();
		builder.AppendLine("Name: " + pilot.pilotName);
		builder.AppendLine("Ship: " + pilot.ship.shipName);
		builder.AppendLine("Level: " + pilot.level);
		builder.AppendLine("Experience: " + pilot.xp);

		if (string.IsNullOrEmpty(pilot.description))
		{
			pilot.description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque tortor dui, elementum eu convallis non, cursus ac dolor. Quisque dictum est quam, et pellentesque velit rutrum eget. Nullam interdum ultricies velit pharetra aliquet. Integer sodales a magna quis ornare. Ut vulputate nibh ipsum. Vivamus tincidunt nec nisi in fermentum. Mauris consequat mi vel odio consequat, eget gravida urna lobortis. Pellentesque eu ipsum consectetur, pharetra nulla in, consectetur turpis. Curabitur ornare eu nisi tempus varius. Phasellus vel ex mauris. Fusce fermentum mi id elementum gravida.";
		}

		builder.AppendLine("Description: " + pilot.description);
		builder.AppendLine("Missions completed: " + pilot.missionsCompleted);
		return builder.ToString();
	}

	private void HirePilot(Pilot pilot)
    {
		if (pilot != null && PlayerManager.Instance.CanSpendMoney(pilot.hireCost))
        {
			PlayerManager.Instance.SpendMoney(pilot.hireCost);
			PilotsManager.Instance.HirePilot(pilot.id);
			hireButton.interactable = false;
			hireButton.GetComponentInChildren<Text>().text = "Pilot Hired!";
			PopulateScrollViews();
        }
    }
}
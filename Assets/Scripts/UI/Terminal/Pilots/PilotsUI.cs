using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 

public class PilotsUI : MonoBehaviour
{
	public GameObject crewPanel;
	public GameObject hiredPilotsList;
	public GameObject pilotsForHireList; 
	public Transform hiredPilotsScrollViewContent;
	public Transform pilotsForHireScrollViewContent;

	public GameObject crewItemPrefab;
	public GameObject buttonPrefab;
	public GameObject backButtonPrefab;
	public GameObject hireButtonPrefab; 

	private GameObject pilotProfilePanel;
	private Text pilotDetailsText; 
	private Image pilotAvatar;
	private Image shipAvatar; 

	public PilotsContainer pilotsContainer;

	private void Awake()
	{
		GeneratePilotsUI();
    }

    private void OnEnable()
    {
		TogglePilotLists();
		pilotProfilePanel.SetActive(false);
    }

    private void GeneratePilotsUI()
    {
		PopulateScrollView(pilotsContainer.pilots, hiredPilotsScrollViewContent);
		PopulateScrollView(PilotsManager.Instance.GetPilotsForHire(), pilotsForHireScrollViewContent);

		GeneratePilotProfilePanel();
        GenerateShipAvatar();
        GeneratePilotAvatar();
		GeneratePilotDetails();
        GeneratePilotProfileButton(backButtonPrefab, PilotsConstants.backButtonAnchors, BackToPilotList);
    }	

	private void PopulateScrollView(Pilot[] pilots, Transform scrollViewContent)
	{
		if (pilots != null)
        {
			foreach (Pilot pilot in pilots)
			{
				GameObject crewItem = Instantiate(crewItemPrefab, scrollViewContent);
				crewItem.GetComponent<Button>().AddOnClick(() => OpenPilotProfilePanel(pilot));
				crewItem.GetComponentInChildren<Text>().text = pilot.pilotName;
			}
        }
	}
	
	private void OpenPilotProfilePanel(Pilot pilot)
	{
		TogglePilotLists();
		pilotProfilePanel.SetActive(true);
		shipAvatar.sprite = pilot.ship.shipAvatar;
		pilotAvatar.sprite = pilot.avatar;
		pilotDetailsText.text = BuildDetailsString(pilot);

		// If the pilot doesn't already work for us, then set up a button to handle hiring him 
		if (!pilot.isHired)
        {
			GeneratePilotProfileButton(hireButtonPrefab, PilotsConstants.hireButtonAnchors, () => HirePilot(pilot));
		}
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

	private void GeneratePilotProfileButton(GameObject prefab, (Vector2, Vector2) anchors, UnityAction callback)
    {
		GameObject newButton = Instantiate(prefab);

        if (pilotProfilePanel != null)
        {
			newButton.transform.SetParent(pilotProfilePanel.transform);
        }

        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.Reset();
        rectTransform.SetAnchors(anchors);

        newButton.GetComponent<Button>().AddOnClick(callback);
    }

	private void HirePilot(Pilot pilot)
    {
		if (pilot != null && PlayerManager.Instance.CanSpendMoney(pilot.hireCost))
        {
			PlayerManager.Instance.SpendMoney(pilot.hireCost);
			PilotsManager.Instance.HirePilot(pilot);
			Debug.Log("Pilot hired: " + pilot.pilotName);
        }
    }
	
	private void BackToPilotList()
	{
		TogglePilotLists();
		pilotProfilePanel.SetActive(false); 
	}

	private void TogglePilotLists()
    {
		hiredPilotsList.SetActive(!hiredPilotsList.activeSelf);
		pilotsForHireList.SetActive(!pilotsForHireList.activeSelf);
    }
}
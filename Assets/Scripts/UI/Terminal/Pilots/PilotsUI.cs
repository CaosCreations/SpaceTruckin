using System.Text;
using UnityEngine;
using UnityEngine.UI; 

public class PilotsUI : MonoBehaviour
{
	public GameObject crewPanel;
	public GameObject crewItemPrefab;
	public Transform scrollViewContent;

	public GameObject pilotButtonPrefab;
	public PilotsContainer pilotsContainer;

	public GameObject pilotProfilePanel;
	private Text pilotDetailsText; 
	private Image pilotAvatar;
	private Image shipAvatar; 


	private void Awake()
	{
		GeneratePilotsUI();

		// Close the pilot profile panel if the player navigates away from it
		// without clicking the back button 
		TerminalUIManager.onTabButtonClicked += ClosePilotProfilePanel;
    }

	private void GeneratePilotsUI()
    {
		GeneratePilotProfilePanel();
        GenerateShipAvatar();
        GeneratePilotAvatar();
		GeneratePilotDetails();
        GenerateBackButton();
		PopulateScrollView();
    }

	private void PopulateScrollView()
	{
		foreach (Pilot pilot in pilotsContainer.pilots)
        {
			GameObject crewItem = Instantiate(crewItemPrefab, scrollViewContent);
			crewItem.GetComponent<Button>().AddOnClick(() => OpenPilotProfilePanel(pilot));
			crewItem.GetComponentInChildren<Text>().text = pilot.pilotName;
        }
	}

	private void GeneratePilotProfilePanel()
	{
		pilotProfilePanel = new GameObject(PilotsConstants.profilePanelName);

		// Sibling of the crew panel so that it can be active while the crew panel is inactive. 
		pilotProfilePanel.transform.parent = crewPanel.transform.parent.transform;

		RectTransform rectTransform = pilotProfilePanel.AddComponent<RectTransform>();
		rectTransform.Reset();
		rectTransform.Stretch();

		pilotProfilePanel.SetActive(false);
	}

	private void OpenPilotProfilePanel(Pilot pilot)
	{
		crewPanel.SetActive(false);
        pilotProfilePanel.SetActive(true);
		shipAvatar.sprite = pilot.ship.shipAvatar;
		pilotAvatar.sprite = pilot.avatar;
		pilotDetailsText.text = BuildDetailsString(pilot);
    }

	private void GeneratePilotDetails()
    {
		GameObject pilotDetails = new GameObject().ScaffoldUI(
			PilotsConstants.detailsObjectName, pilotProfilePanel, PilotsConstants.pilotDetailsAnchors);

		pilotDetails.GetComponent<RectTransform>().SetPadding(Side.Top, PilotsConstants.topPadding);
		pilotDetailsText = pilotDetails.AddComponent<Text>();
		pilotDetailsText.SetDefaultFont();
		pilotDetailsText.color = Color.black;
	}

	private string BuildDetailsString(Pilot pilot)
	{
		StringBuilder builder = new StringBuilder();
		builder.AppendLine("Name: " + pilot.name);
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

	private void GeneratePilotAvatar()
    {
		GameObject pilotAvatarObject = new GameObject().ScaffoldUI(
			PilotsConstants.pilotAvatarObjectName, pilotProfilePanel, PilotsConstants.pilotAvatarAnchors);

		pilotAvatar = pilotAvatarObject.AddComponent<Image>();
	}

	private void GenerateShipAvatar()
    {
		GameObject shipAvatarObject = new GameObject().ScaffoldUI(
			PilotsConstants.shipAvatarObjectName, pilotProfilePanel, PilotsConstants.shipAvatarAnchors);

		shipAvatar = shipAvatarObject.AddComponent<Image>();

    }

	private void GenerateBackButton()
    {
		GameObject backButton = Instantiate(pilotButtonPrefab);
		backButton.name = PilotsConstants.backButtonName;

		if (pilotProfilePanel != null)
        {
			backButton.transform.parent = pilotProfilePanel.transform; 
        }

		RectTransform rectTransform = backButton.GetComponent<RectTransform>();
		rectTransform.Reset();
		rectTransform.SetAnchors(PilotsConstants.backButtonAnchors);

		Button button = backButton.GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(delegate { ClosePilotProfilePanel(); });
		backButton.GetComponentInChildren<Text>().text = PilotsConstants.backButtonText; 
    }
	
	private void ClosePilotProfilePanel()
	{
		crewPanel.SetActive(true);
		pilotProfilePanel.SetActive(false); 
	}
}
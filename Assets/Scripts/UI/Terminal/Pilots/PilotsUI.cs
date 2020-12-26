using UnityEngine;
using UnityEngine.UI; 

public class PilotsUI : MonoBehaviour
{
	public GameObject crewPanel;
	public GameObject crewItemPrefab;
	public Transform scrollViewContent;

	public GameObject pilotButtonPrefab;
	public PilotsContainer pilotsContainer;

	private GameObject pilotProfilePanel;
	private Text pilotNameText;
	private Text pilotDescriptionText; 
	private Image pilotAvatar;
	private Image shipAvatar; 

	private void Awake()
	{
		GeneratePilotsUI(); 
    }

	private void GeneratePilotsUI()
    {
		GeneratePilotProfilePanel();
		PopulateScrollView();
		//GeneratePilotButtons();
		//GeneratePilotNameText();
		//GeneratePilotDescription();
		//GeneratePilotAvatar();
		//GenerateShipAvatar();
		//GenerateBackButton(); 
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

	private void GeneratePilotButtons()
	{
		GameObject pilotButtonGroup = new GameObject(PilotsConstants.buttonGroupName);
		pilotButtonGroup.transform.parent = crewPanel.transform;
		RectTransform rectTransform = pilotButtonGroup.AddComponent<RectTransform>();
        rectTransform.localPosition = Vector2.zero;
        rectTransform.SetAnchors((PilotsConstants.buttonGroupAnchorMin, PilotsConstants.buttonGroupAnchorMax));

        VerticalLayoutGroup verticalLayoutGroup = pilotButtonGroup.AddComponent<VerticalLayoutGroup>();
		verticalLayoutGroup.childControlWidth = true;
		verticalLayoutGroup.childControlHeight = true;

		foreach (Pilot pilot in pilotsContainer.pilots)
		{
			GameObject pilotButton = Instantiate(pilotButtonPrefab, pilotButtonGroup.transform);
			pilotButton.name = $"{pilot.pilotName}Button"; 
			pilotButton.GetComponentInChildren<Text>().text = pilot.pilotName;
			Button button = pilotButton.GetComponent<Button>();
			button.onClick.RemoveAllListeners(); 
			button.onClick.AddListener(delegate { OpenPilotProfilePanel(pilot); });

			// These will be set by the hire pilots logic later on
			pilot.hired = true; 
			pilot.onMission = false; 
		}
	}

	private void OpenPilotProfilePanel(Pilot pilot)
	{
		crewPanel.SetActive(false);
        pilotProfilePanel.SetActive(true);
		pilotNameText.text = pilot.pilotName;
		pilotDescriptionText.text = pilot.description;
		pilotAvatar.sprite = pilot.avatar;
		shipAvatar.sprite = pilot.ship.shipAvatar;
    }

	private void GeneratePilotNameText()
    {
		GameObject pilotNameObject = new GameObject(PilotsConstants.nameTextName);
		pilotNameObject.transform.parent = pilotProfilePanel.transform;

		RectTransform rectTransform = pilotNameObject.AddComponent<RectTransform>();
		rectTransform.Reset();
		rectTransform.SetAnchors((PilotsConstants.nameTextAnchorMin, PilotsConstants.nameTextAnchorMax));

		pilotNameText = pilotNameObject.AddComponent<Text>();
		pilotNameText.SetDefaultFont();
	}

	private void GeneratePilotDescription()
    {
		GameObject pilotDescriptionObject = new GameObject(PilotsConstants.descriptionObjectName);
		pilotDescriptionObject.transform.parent = pilotProfilePanel.transform;

		RectTransform rectTransform = pilotDescriptionObject.AddComponent<RectTransform>();
		rectTransform.Reset();
		rectTransform.SetAnchors((PilotsConstants.descriptionAnchorMin, PilotsConstants.descriptionAnchorMax));

		pilotDescriptionText = pilotDescriptionObject.AddComponent<Text>();
		pilotDescriptionText.SetDefaultFont();
	}

	private void GeneratePilotAvatar()
    {
		GameObject pilotAvatarObject = new GameObject().ScaffoldUI(
			PilotsConstants.pilotAvatarObjectName, pilotProfilePanel, (PilotsConstants.avatarAnchorMin, PilotsConstants.avatarAnchorMax));

		pilotAvatar = pilotAvatarObject.AddComponent<Image>();
	}

	private void GenerateShipAvatar()
    {
		GameObject shipAvatarObject = new GameObject().ScaffoldUI(
			PilotsConstants.shipAvatarObjectName, pilotProfilePanel, (Vector2.zero, new Vector2(0.66f, 1f)));

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
		rectTransform.SetAnchors((PilotsConstants.backButtonAnchorMin, PilotsConstants.backButtonAnchorMax));

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
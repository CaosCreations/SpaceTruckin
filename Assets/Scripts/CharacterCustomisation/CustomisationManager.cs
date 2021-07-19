using UnityEngine;
using UnityEngine.UI;

public class CustomisationManager : MonoBehaviour
{
    public CustomisationTypeContainer customisationTypeContainer;
    public GameObject leftArrowPrefab;
    public GameObject rightArrowPrefab;
    private GameObject customisationContainer;

    private readonly Color[] colors =
    {
        Color.red, Color.green, Color.blue, Color.yellow, Color.white, Color.black, Color.cyan, Color.grey
    };

    public enum Direction { Left = 0, Right = 1 };

    private void Start()
    {
        GenerateCustomisationOptions();
        LogCustomisationState();
    }

    private void GenerateCustomisationOptions()
    {
        // This is the overarching parent object 
        customisationContainer = new GameObject(CustomisationConstants.CustomisationContainerName);
        customisationContainer.transform.SetParent(gameObject.transform);
        customisationContainer.transform.localPosition = Vector3.zero;

        RectTransform rectTransform = customisationContainer.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(1f, 1f);
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        VerticalLayoutGroup verticalLayoutGroup = customisationContainer.AddComponent<VerticalLayoutGroup>();
        verticalLayoutGroup.spacing = CustomisationConstants.CustomisationContainerSpacing;

        foreach (CustomisationType customisationType in customisationTypeContainer.CustomisationTypes)
        {
            // This is the parent object for each individual row of the customisation container  
            GameObject customisationOption = new GameObject(CustomisationConstants.CustomisationOptionName);
            customisationOption.transform.SetParent(customisationContainer.transform);
            RectTransform customisationOptionRT = customisationOption.AddComponent<RectTransform>();
            customisationOption.AddComponent<HorizontalLayoutGroup>();

            GameObject leftArrow = Instantiate(leftArrowPrefab);
            leftArrow.transform.SetParent(customisationOption.transform);
            leftArrow.transform.localScale = new Vector2(0.25f, 0.25f);
            Button leftArrowBtn = leftArrow.GetComponent<Button>();
            leftArrowBtn.onClick.RemoveAllListeners();

            GameObject imageObject = new GameObject(CustomisationConstants.CustomisationImageName);
            imageObject.transform.SetParent(customisationOption.transform);
            imageObject.transform.localScale = Vector2.one;
            customisationType.Image = imageObject.AddComponent<Image>();
            customisationType.Image.sprite = customisationType.Sprite;
            customisationType.Image.color = customisationType.Color;

            GameObject rightArrow = Instantiate(rightArrowPrefab);
            rightArrow.transform.SetParent(customisationOption.transform);
            rightArrow.transform.localScale = new Vector2(0.25f, 0.25f);
            Button rightArrowBtn = rightArrow.GetComponent<Button>();
            rightArrowBtn.onClick.RemoveAllListeners();

            leftArrowBtn.onClick.AddListener(delegate { CycleThroughOptions(customisationType, Direction.Left); });
            rightArrowBtn.onClick.AddListener(delegate { CycleThroughOptions(customisationType, Direction.Right); });
        }
    }

    private void CycleThroughOptions(CustomisationType customisationType, Direction direction)
    {
        if (direction == Direction.Left)
        {
            // This wraps around if the user reaches the end of the customisation options for that group 
            if (customisationType.Index > 0)
            {
                customisationType.Index--;
            }
            else
            {
                customisationType.Index = colors.Length - 1;
            }
        }
        else if (direction == Direction.Right)
        {
            if (customisationType.Index < colors.Length - 1)
            {
                customisationType.Index++;
            }
            else
            {
                customisationType.Index = 0;
            }
        }
        customisationType.Color = colors[customisationType.Index];
        customisationType.Image.color = customisationType.Color;

        LogCustomisationState();
    }

    void LogCustomisationState()
    {
        for (int i = 0; i < customisationTypeContainer.CustomisationTypes.Length; i++)
        {
            Debug.Log($"Customisation Type {i} value: {customisationTypeContainer.CustomisationTypes[i].Index}");
        }
    }
}

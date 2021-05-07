using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationUI : MonoBehaviour
{
    [SerializeField] private InputField characterNameInput;
    [SerializeField] private Text invalidInputText;
    [SerializeField] private Button okButton;

    private string CharacterName 
    {
        get => characterNameInput.text; set => characterNameInput.text = value; 
    } 

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        AddListeners();
        invalidInputText.SetActive(false);
    }

    private void AddListeners()
    {
        okButton.AddOnClick(ChooseName);

        characterNameInput.onValidateInput += (string input, int charIndex, char addedChar) =>
        {
            return UIUtils.ValidateCharInput(addedChar, UIConstants.AlphabeticalIncludingAccentsPattern);
        };

        characterNameInput.AddOnValueChanged(() =>
        {
            CharacterName = CharacterName
                .TrimStart()
                .RemoveConsecutiveSpaces()
                .EnforceCharacterLimit(PlayerConstants.MaxPlayerNameLength);
        });
    }

    private void ChooseName()
    {
        if (NameIsValid(CharacterName))
        {
            PlayerManager.SetPlayerName(CharacterName);
            invalidInputText.SetActive(false);
            UIManager.ClearCanvases();
        }
        else
        {
            Debug.LogError(
                $"Invalid input when choosing name (must be alphabetical string and not only whitespace). Value was: '{CharacterName}'");

            invalidInputText.SetActive(true);
        }
    }

    private bool NameIsValid(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.IsAlphabetical(includeAccents: true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChooseName();
        }
    }
}
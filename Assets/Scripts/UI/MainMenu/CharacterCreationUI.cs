using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationUI : MonoBehaviour
{
    [SerializeField] protected InputField characterNameInput;
    [SerializeField] protected Text invalidInputText;
    [SerializeField] protected Button okButton;

    protected string CharacterName
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

    protected virtual void AddListeners()
    {
        okButton.AddOnClick(ChooseName);

        AddValidationListener();
        AddFormattingListener();
    }

    protected void AddValidationListener()
    {
        characterNameInput.onValidateInput += (string input, int charIndex, char addedChar) =>
        {
            return UIUtils.ValidateCharInput(addedChar, UIConstants.AlphabeticalIncludingAccentsPattern);
        };
    }

    protected void AddFormattingListener()
    {
        characterNameInput.AddOnValueChanged(() =>
        {
            CharacterName = CharacterName
                .TrimStart()
                .RemoveConsecutiveSpaces()
                .EnforceCharacterLimit(PlayerConstants.MaxPlayerNameLength);
        });
    }

    protected void ChooseName()
    {
        if (NameIsValid(CharacterName))
        {
            PlayerManager.SetPlayerName(CharacterName);
            invalidInputText.SetActive(false);

            if (UIManager.Instance != null)
                UIManager.ClearCanvases();
        }
        else
        {
            Debug.LogError(
                $"Invalid input when choosing name (must be alphabetical string and not only whitespace). Value was: '{CharacterName}'");

            invalidInputText.SetActive(true);
        }
    }

    protected bool NameIsValid(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.IsAlphabetical(includeAccents: true);
    }

    protected virtual void Update()
    {
        if (UIManager.GetNonOverriddenKeyDown(PlayerConstants.ChooseNameKey))
        {
            ChooseName();
        }
    }
}
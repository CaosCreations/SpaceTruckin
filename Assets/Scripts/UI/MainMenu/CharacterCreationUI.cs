using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationUI : MonoBehaviour
{
    [SerializeField] protected InputField characterNameInput;
    [SerializeField] protected Text invalidInputText;
    [SerializeField] protected Button okButton;
    protected bool isNameValid;

    protected string CharacterName
    {
        get => characterNameInput.text; set => characterNameInput.text = value;
    }

    protected virtual void Start()
    {
        invalidInputText.SetActive(false);
        CharacterName = string.Empty;

        characterNameInput.onValueChanged.RemoveAllListeners();
        okButton.AddOnClick(ChooseName, removeListeners: false);

        characterNameInput.AddOnValueChanged(OnValueChanged);
        characterNameInput.onValidateInput += (string input, int charIndex, char addedChar) =>
        {
            return UIUtils.ValidateCharInput(addedChar, UIConstants.AlphabeticalIncludingAccentsPattern);
        };
    }

    protected virtual void OnValueChanged()
    {
        CharacterName = CharacterName
        .TrimStart()
        .RemoveConsecutiveSpaces()
        .EnforceCharacterLimit(PlayerConstants.MaxPlayerNameLength);

        isNameValid = IsNameValid(CharacterName);
    }

    protected void ChooseName()
    {
        if (IsNameValid(CharacterName))
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

    protected bool IsNameValid(string name)
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
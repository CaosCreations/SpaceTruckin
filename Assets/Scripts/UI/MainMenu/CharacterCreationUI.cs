using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationUI : MonoBehaviour
{
    [SerializeField] private InputField characterNameInput;
    [SerializeField] private Text invalidInputText;
    [SerializeField] private Button okButton;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        okButton.AddOnClick(ChooseName);
        invalidInputText.SetActive(false);

        characterNameInput.onValidateInput += (string input, int charIndex, char addedChar) =>
        {
            return UIUtils.ValidateCharInput(addedChar, UIConstants.AlphabeticalIncludingAccentsPattern);
        };

        characterNameInput.AddOnValueChanged(() =>
        {
            characterNameInput.text = characterNameInput.text.RemoveTrailingDoubleSpace();
        });
    }

    private void ChooseName()
    {
        string choiceOfName = characterNameInput.text;

        if (NameIsValid(choiceOfName))
        {
            PlayerManager.SetPlayerName(choiceOfName);
            invalidInputText.SetActive(false);
            UIManager.ClearCanvases();
        }
        else
        {
            Debug.LogError(
                $"Invalid input when choosing name (must be alphabetical string and not only whitespace). Value was: '{choiceOfName}'");

            invalidInputText.SetActive(true);
        }
    }

    private bool NameIsValid(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.IsAlphabeticalIncludingAccents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChooseName();
        }
    }
}
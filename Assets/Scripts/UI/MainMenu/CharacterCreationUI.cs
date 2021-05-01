using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationUI : MonoBehaviour
{
    [SerializeField] private InputField characterNameInput;
    [SerializeField] private Text invalidInputText; 
    [SerializeField] private Button okButton;

    private void Start()
    {
        okButton.AddOnClick(ChooseName);
        invalidInputText.SetActive(false);
    }

    private void ChooseName()
    {
        string choiceOfName = characterNameInput.text;
        
        if (NameIsValid(choiceOfName))
        {
            PlayerManager.SetPlayerName(choiceOfName);
            invalidInputText.SetActive(false);
            UIManager.ClearCanvases();
            UIManager.Instance.currentMenuOverridesEscape = false;
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
        return !string.IsNullOrWhiteSpace(name) && name.IsAlphabetical();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChooseName();
        }
    }
}
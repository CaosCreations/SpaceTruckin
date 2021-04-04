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
        
        if (choiceOfName.IsAlphabetical())
        {
            PlayerManager.SetPlayerName(choiceOfName);
            invalidInputText.SetActive(false);
            UIManager.ClearCanvases();
        }
        else
        {
            Debug.LogError(
                $"Invalid input when choosing name (must be alphabetical string). Value was: {choiceOfName}");
            
            invalidInputText.SetActive(true);
        }
    }
}
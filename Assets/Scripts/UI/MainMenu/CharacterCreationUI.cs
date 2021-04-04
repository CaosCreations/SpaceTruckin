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
    }

    private void ChooseName()
    {
        string choiceOfName = characterNameInput.text;
        
        if (choiceOfName.IsAlphabetical())
        {
            PlayerManager.SetPlayerName(characterNameInput.text);
        }
        else
        {
            Debug.LogError(
                $"Invalid input when choosing name (must be alphabetical string). Value was: {characterNameInput.text}");
            
            invalidInputText.SetActive(true);
        }
    }
}
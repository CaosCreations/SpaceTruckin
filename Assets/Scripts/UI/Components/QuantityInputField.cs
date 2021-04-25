using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class QuantityInputField : MonoBehaviour
{
    [SerializeField] private InputField inputField;

    private void Awake()
    {
        inputField.onValidateInput += (string input, int charIndex, char addedChar) =>
        {
            return ValidateChar(addedChar);
        };
    }

    private char ValidateChar(char addedChar)
    {
        if (Regex.IsMatch(addedChar.ToString(), UIConstants.UnsignedIntegerPattern)) 
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }
}

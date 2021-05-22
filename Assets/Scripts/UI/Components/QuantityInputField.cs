using UnityEngine;
using UnityEngine.UI;

public class QuantityInputField : MonoBehaviour
{
    [SerializeField] private InputField inputField;

    private void Awake()
    {
        inputField.onValidateInput += (string input, int charIndex, char addedChar) =>
        {
            return UIUtils.ValidateCharInput(addedChar, UIConstants.UnsignedIntegerPattern);
        };
    }
}

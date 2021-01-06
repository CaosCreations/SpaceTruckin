using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyboardNavigation : MonoBehaviour
{
    public TerminalUIManager terminalManager;
    private ArrayList navigableButtons;
    private int currentIndex;

    private Color selectedColour = Color.green;
    private Color unselectedColour = Color.white;

    private void Start()
    {
        navigableButtons = GetNavigableButtons();
        HighlightButton(currentIndex);

    }

    private ArrayList GetNavigableButtons()
    {
        ArrayList navigableButtons = new ArrayList();

        foreach (FieldInfo field in terminalManager.GetType().GetFields())
        {
            if (field.GetValue(terminalManager) is Button)
            {
                Debug.Log("Field is button");

                // Todo: Casting properly 
                navigableButtons.Add(field.GetValue(terminalManager) as Button);
            }
        }
        return navigableButtons;
    }

    private void HighlightButton(int index)
    {
        var selectedButton = navigableButtons[index] as Button;
        selectedButton.GetComponent<Image>().color = selectedColour;
    }

    private void UnhighlightButton(int index)
    {
        var selectedButton = navigableButtons[index] as Button;
        selectedButton.GetComponent<Image>().color = unselectedColour;
    }

    // Todo: This is so ugly... But it works
    private void NavigateButtons()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            UnhighlightButton(currentIndex);
            currentIndex++;

            if (currentIndex >= navigableButtons.Count)
            {
                UnhighlightButton(navigableButtons.Count - 1);
                currentIndex = 0;
            }
            else
            {
            }
            // Todo: Only call this once...
            HighlightButton(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UnhighlightButton(currentIndex);
            currentIndex--;

            if (currentIndex < 0)
            {
                UnhighlightButton(0);
                currentIndex = navigableButtons.Count - 1;
            }
            HighlightButton(currentIndex);
        }
    }

    private void Update()
    {
        NavigateButtons();

        // Todo: Encapsulate; mesh better with the other polling methods 
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Button button = navigableButtons[currentIndex] as Button;
            button.onClick.Invoke();
        }
    }




}

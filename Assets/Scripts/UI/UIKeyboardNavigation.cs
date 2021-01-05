using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyboardNavigation : MonoBehaviour
{
    public TerminalUIManager terminalManager;
    private ArrayList navigableButtons; 

    private void Start()
    {
        navigableButtons = GetNavigableButtons();
    }

    private ArrayList GetNavigableButtons()
    {
        ArrayList navigableButtons = new ArrayList();

        foreach (FieldInfo field in terminalManager.GetType().GetFields())
        {
            if (field.GetValue(terminalManager) is Button)
            {
                Debug.Log("Field is button");
                navigableButtons.Add(field.GetValue(terminalManager));
            }
        }
        return navigableButtons;
    }

}

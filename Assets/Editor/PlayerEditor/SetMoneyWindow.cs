using System;
using UnityEditor;
using UnityEngine;

public class SetMoneyWindow : EditorWindow
{
    private long amount;

    [MenuItem("Space Truckin/Player/Set Money")]
    private static void Init()
    {
        EditorWindow window = GetWindow(typeof(SetMoneyWindow));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUI.DropShadowLabel(
            new Rect(0, 0, position.width - 40, 20), "Enter amount: ");

        amount = Math.Max(
            EditorGUI.LongField(new Rect(20, 20, position.width - 40, 20), amount), 0);

        if (GUI.Button(new Rect(20, 40, position.width - 40, 20), "Set Money"))
        {
            PlayerEditor.SetMoney(Math.Max(0, amount));
            SetMoneyTexts();
        }

        Repaint();
    }

    private static void SetMoneyTexts()
    {
        try
        {
            var moneyTexts = FindObjectsOfType<MoneyText>();
            foreach (var text in moneyTexts)
            {
                text.UpdateMoneyText();
            }
        }
        catch (Exception)
        {
            Debug.Log("Unable to set money texts");
        }
    }
}
using System;
using UnityEditor;
using UnityEngine;

public class SetMoneyWindow : EditorWindow
{
    public static event Action OnMoneySet;
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
            OnMoneySet?.Invoke();
        }

        Repaint();
    }



}
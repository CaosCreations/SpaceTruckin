// Copyright (c) Pixel Crushers. All rights reserved.

using System.IO;
using UnityEngine;
using UnityEditor;

namespace PixelCrushers
{

    /// <summary>
    /// This script runs when Unity starts or reloads assemblies after compilation.
    /// If it hasn't yet asked, it asks to set the InputDeviceManager standard inputs.
    /// 
    /// Note: Also adds TMP_PRESENT if using Unity 6.1+.
    /// </summary>
    [InitializeOnLoad]
    public static class CheckInputManagerSettings
    {

        private const string TMP_PRESENT = "TMP_PRESENT";
        private const string USE_NEW_INPUT = "USE_NEW_INPUT";

        private const string CheckedFlagFilename = "Plugins/Pixel Crushers/Common/checked.txt";

        static CheckInputManagerSettings()
        {
            var filename = $"{Application.dataPath}/{CheckedFlagFilename}";
            var hasAlreadyChecked = File.Exists(filename);
            if (hasAlreadyChecked) return;

            File.WriteAllText(filename, $"Checked TMPro & Input System defines on {System.DateTime.Now}");

            Check_TMP_PRESENT();
            Check_Input_System();
        }

        private static void Check_TMP_PRESENT()
        {
#if UNITY_6000_0_OR_NEWER && !TMP_PRESENT
            MoreEditorUtility.TryAddScriptingDefineSymbols(TMP_PRESENT);
            Debug.Log("Added Scripting Define Symbol TMP_PRESENT to support TextMesh Pro.");
#endif
        }

        private static void Check_Input_System()
        {
#if ENABLE_INPUT_SYSTEM && !USE_NEW_INPUT
            if (EditorUtility.DisplayDialog("Enable Input System Support?",
                "Do you want to enable Input System package support for this Pixel Crushers asset?", "Yes", "No"))
            {
                MoreEditorUtility.TryAddScriptingDefineSymbols(USE_NEW_INPUT);
                Debug.Log("Added Scripting Define Symbol USE_NEW_INPUT to support the Input System package.");
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (InputDeviceManagerEditor.HasStandardInputDefinitions()) return;
            if (EditorUtility.DisplayDialog("Add Input Manager Settings?",
                "Do you want to add standard legacy input manager definitions for joystick axes so the Input Device Manager can detect when the player is using a joystick?", "Yes", "No"))
            {
                InputDeviceManagerEditor.AddStandardInputDefinitions();
                Debug.Log("Added standard input definitions.");
            }
#endif
        }

    }
}

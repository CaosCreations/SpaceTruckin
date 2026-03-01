#if !USE_YARN3

// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.DialogueSystem.Yarn3
{

    public class Yarn3ImporterWindow
    {

        [MenuItem("Tools/Pixel Crushers/Dialogue System/Import/Yarn/Yarn 3...", false, 1)]
        public static void AskEnableYarnSupport()
        {
            if (EditorUtility.DisplayDialog("Enable Yarn 3 Support", "Yarn 3 import support isn't enabled yet. Would you like to enable it?\n\nYarn Spinner 3 and the ANTLR4 Runtime MUST be installed in your project first, and you must configure the Dialogue System assembly definitions to reference it. If you haven't set this up yet, click Cancel and refer to the Yarn 3 Import manual section.\n\nAfter clicking Enable, re-open the Yarn 3 import window.", "Enable", "Cancel"))
            {
                EditorTools.TryAddScriptingDefineSymbols("USE_YARN3");
            }
        }
    }
}
#endif

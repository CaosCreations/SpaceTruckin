#if UNITY_2021_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Implements a required class for UIToolkitDialogueUI to be able to inherit from AbstractDialogueUI.
    /// </summary>
    [Serializable]
    public class UIToolkitRootElements : AbstractUIRoot
    {

        public override void Hide() { }
        public override void Show() { }

    }

}
#endif

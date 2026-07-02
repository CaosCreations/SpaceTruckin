#if UNITY_2022_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Colors to use for quest headings and entries based on their states.
    /// </summary>
    [System.Serializable]
    public class UIToolkitQuestStateColors
    {

        [SerializeField] private Color active = Color.white;
        [SerializeField] private Color success = Color.green;
        [SerializeField] private Color failure = Color.red;

        public Color Active => active;
        public Color Success => success;
        public Color Failure => failure;

        public Color GetColor(QuestState state)
        {
            switch (state)
            {
                default:
                    return Active;
                case QuestState.Success:
                    return Success;
                case QuestState.Failure:
                    return Failure;
            }
        }

    }

}
#endif

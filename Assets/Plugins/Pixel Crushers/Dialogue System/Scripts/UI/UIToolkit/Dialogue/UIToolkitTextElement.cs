#if UNITY_2022_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// This is an abstraction layer for UI Toolkit text elements that can be either
    /// standard Label elements or Febucci Text Animator for Unity AnimatedLabels.
    /// To support Text Animator for Unity, you must define the scripting symbol
    /// USE_TEXT_ANIMATOR
    /// </summary>
    public class TextElement
    {
        public UIDocument document;
        public Label label;
#if USE_TEXT_ANIMATOR
        public Febucci.TextAnimatorForUnity.AnimatedLabel animatedLabel;
#endif

        public TextElement(UIDocument document, string elementName)
        {
            this.document = document;
            label = UIToolkitUtility.GetVisualElement<Label>(document, elementName);
#if USE_TEXT_ANIMATOR
            animatedLabel = UIToolkitUtility.GetVisualElement<Febucci.TextAnimatorForUnity.AnimatedLabel>(document, elementName);
#endif
        }

        public string text
        {
            get
            {
                if (label != null) return label.text;
#if USE_TEXT_ANIMATOR
                else if (animatedLabel != null) return animatedLabel.Text;
#endif
                else return string.Empty;
            }
            set
            {
                if (label != null) label.text = value;
#if USE_TEXT_ANIMATOR
                else if (animatedLabel != null) animatedLabel.Text = value;
#endif
            }
        }

        public bool IsVisible
        {
            get
            {
                if (label != null) return UIToolkitUtility.IsVisible(label);
#if USE_TEXT_ANIMATOR
                else if (animatedLabel != null) return UIToolkitUtility.IsVisible(animatedLabel);
#endif
                else return false;
            }
        }

        public bool IsTyping
        {
            get
            {
#if USE_TEXT_ANIMATOR
                if (animatedLabel != null) return animatedLabel.Typewriter.IsShowingText;
#endif
                return false;
            }
        }

        public void SetDisplay(bool value, bool setFocus)
        {
            UIToolkitUtility.SetDisplay(label, value, setFocus);
#if USE_TEXT_ANIMATOR
            UIToolkitUtility.SetDisplay(animatedLabel, value, setFocus);
#endif
        }

        public void FastForwardTypewriterToEnd()
        {
#if USE_TEXT_ANIMATOR
            if (animatedLabel != null) animatedLabel.Typewriter.SkipTypewriter();
#endif
        }

    }

}
#endif

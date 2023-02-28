using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class StefanoDialogueUI : SMSDialogueUI
{
    // Copied from SMSDialogueUI but added:
    // 1. Use actor's node color for subtitle background.
    // 2. Don't add bubbles for blank text.
    protected override void AddMessage(Subtitle subtitle)
    {
        var dialogueActor = GetDialogueActor(subtitle);
        var template = GetTemplate(subtitle, dialogueActor);

        // Set portrait name/image:
        template.SetContent(subtitle);
        var actor = DialogueManager.masterDatabase.GetActor(subtitle.speakerInfo.id);
        var nodeColor = Tools.WebColor(actor.LookupValue("NodeColor"));

        if (template.portraitName.gameObject != null)
        {
            template.portraitName.text = subtitle.speakerInfo.Name;
            var portraitNameImage = template.portraitName.gameObject.transform.parent.GetComponent<Image>();
            portraitNameImage.color = nodeColor;
            template.portraitName.gameObject.SetActive(true);
        }

        // If text is blank, that's all we do:
        if (string.IsNullOrWhiteSpace(subtitle.formattedText.text)) return;

        // The rest is just a copy of the original AddMessage() method:
        var go = Instantiate(template.panel.gameObject) as GameObject;
        var text = subtitle.formattedText.text;
        go.name = (text.Length <= 20) ? text : text.Substring(0, Mathf.Min(20, text.Length)) + "...";
        instantiatedMessages.Add(go);
        go.transform.SetParent(messagePanel.transform, false);
        var panel = go.GetComponent<StandardUISubtitlePanel>();

        // Set panel children image colours
        foreach (Transform item in panel.gameObject.transform)
        {
            if (item.TryGetComponent<Image>(out var image))
                image.color = nodeColor;
        }

        if (panel.addSpeakerName)
        {
            subtitle.formattedText.text = string.Format(panel.addSpeakerNameFormat, new object[] { subtitle.speakerInfo.Name, subtitle.formattedText.text });
        }
        if (dialogueActor != null && dialogueActor.standardDialogueUISettings.setSubtitleColor)
        {
            subtitle.formattedText.text = dialogueActor.AdjustSubtitleColor(subtitle);
        }
        panel.ShowSubtitle(subtitle);
        continueButton = panel.continueButton;
        if (shouldShowContinueButton && !isLoadingGame)
        {
            panel.ShowContinueButton();
        }
        else
        {
            panel.HideContinueButton();
        }
        if (isLoadingGame)
        {
            var typewriter = panel.GetTypewriter();
            if (typewriter != null) typewriter.Stop();
        }
        if (maxMessages > 0 && instantiatedMessages.Count > maxMessages)
        {
            Destroy(instantiatedMessages[0]);
            instantiatedMessages.RemoveAt(0);
        }
        ScrollToBottom(); //--- Now does smooth scroll: StartCoroutine(JumpToBottom());

    }
}

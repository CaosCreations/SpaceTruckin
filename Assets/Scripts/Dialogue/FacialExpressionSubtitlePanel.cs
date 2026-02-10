using PixelCrushers.DialogueSystem;
using UnityEngine;

// Index 0 = default portrait, indices 1-8 = alternate portraits.
public class FacialExpressionSubtitlePanel : StandardUISubtitlePanel
{
    [SerializeField] private CompositePortraitRenderer compositePortrait;
    [SerializeField] private ActorFaceDataRegistry faceDataRegistry;

    private const int NoExpressionOverride = -1;

    public override void SetContent(Subtitle subtitle)
    {
        if (subtitle == null)
        {
            return;
        }

        currentSubtitle = subtitle;
        lastActorID = subtitle.speakerInfo.id;
        CheckSubtitleAnimator(subtitle);

        if (!onlyShowNPCPortraits || subtitle.speakerInfo.isNPC)
        {
            SetPortraitWithFacialExpression(subtitle);
            SetPortraitName(subtitle);
        }

        if (waitForOpen && panelState != PanelState.Open)
        {
            DialogueManager.instance.StartCoroutine(SetSubtitleTextContentAfterOpen(subtitle));
        }
        else
        {
            SetSubtitleTextContent(subtitle);
        }

        frameLastSetContent = Time.frameCount;
    }

    private void SetPortraitWithFacialExpression(Subtitle subtitle)
    {
        if (portraitImage == null)
        {
            return;
        }

        var actorName = subtitle.speakerInfo.nameInDatabase;
        var faceData = faceDataRegistry != null ? faceDataRegistry.GetByActorName(actorName) : null;

        if (faceData != null && compositePortrait != null)
        {
            SetCompositePortrait(subtitle, faceData);
        }
        else
        {
            SetFallbackPortrait(subtitle);
        }

        portraitActorName = actorName;
    }

    private void SetCompositePortrait(Subtitle subtitle, ActorFaceData faceData)
    {
        if (!DialogueUtils.TryGetFaceExpression(subtitle.dialogueEntry, out var expression))
        {
            expression = new FaceExpression { Eye = 0, Mouth = 0 };
        }

        compositePortrait.Show();
        compositePortrait.SetExpression(faceData, expression.EyeIndex, expression.MouthIndex);
        portraitImage.gameObject.SetActive(false);
    }

    private void SetFallbackPortrait(Subtitle subtitle)
    {
        if (compositePortrait != null)
        {
            compositePortrait.Hide();
        }

        portraitImage.gameObject.SetActive(true);

        var expressionIndex = DialogueUtils.GetFacialExpressionIndex(subtitle.dialogueEntry, NoExpressionOverride);
        var sprite = GetPortraitForExpression(subtitle.speakerInfo, expressionIndex);
        SetPortraitImage(sprite);
    }

    private void SetPortraitName(Subtitle subtitle)
    {
        if (portraitName.text != subtitle.speakerInfo.Name)
        {
            portraitName.text = subtitle.speakerInfo.Name;
            UITools.SendTextChangeMessage(portraitName);
        }
    }

    private Sprite GetPortraitForExpression(PixelCrushers.DialogueSystem.CharacterInfo speakerInfo, int expressionIndex)
    {
        if (expressionIndex == NoExpressionOverride)
        {
            return currentSubtitle.GetSpeakerPortrait();
        }

        // Convert user index (0-8) to pic number (1-9) for GetPicOverride
        // Index 0 = default portrait (picNum 1)
        // Index 1-8 = alternate portraits (picNum 2-9)
        int picNumber = expressionIndex + 1;
        return speakerInfo.GetPicOverride(picNumber);
    }
}

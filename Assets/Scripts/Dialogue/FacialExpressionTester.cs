using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class FacialExpressionTester : MonoBehaviour
{
    [Tooltip("Name of the actor in the Dialogue Database")]
    [SerializeField]
    private string actorName;

    [Header("Expression")]
    [Tooltip("Current facial expression index (0-8)")]
    [Range(0, 8)]
    [SerializeField]
    private int expressionIndex;

    [Header("Display")]
    [Tooltip("Image component to display the portrait (optional, will search on this GameObject if not set)")]
    [SerializeField]
    private Image portraitImage;

    private Actor cachedActor;
    private int lastExpressionIndex = -1;
    private string lastActorName;

#if UNITY_EDITOR
    [Header("Editor")]
    [SerializeField]
    private bool autoRefresh = true;
#endif

    private void Start()
    {
        if (portraitImage == null)
        {
            portraitImage = GetComponent<Image>();
        }

        RefreshPortrait();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (autoRefresh && (expressionIndex != lastExpressionIndex || actorName != lastActorName))
        {
            RefreshPortrait();
        }
#endif
    }

    [ContextMenu("Refresh Portrait")]
    public void RefreshPortrait()
    {
        if (portraitImage == null)
        {
            Debug.LogWarning("FacialExpressionTester: No Image component assigned or found.", this);
            return;
        }

        if (string.IsNullOrEmpty(actorName))
        {
            Debug.LogWarning("FacialExpressionTester: Actor name is empty.", this);
            return;
        }

        if (DialogueManager.masterDatabase == null)
        {
            Debug.LogWarning("FacialExpressionTester: No Dialogue Database loaded.", this);
            return;
        }

        if (cachedActor == null || lastActorName != actorName)
        {
            cachedActor = DialogueManager.masterDatabase.GetActor(actorName);
            lastActorName = actorName;
        }

        if (cachedActor == null)
        {
            Debug.LogWarning($"FacialExpressionTester: Actor '{actorName}' not found in database.", this);
            return;
        }

        var sprite = GetPortraitSprite(cachedActor, expressionIndex);
        portraitImage.sprite = sprite;
        portraitImage.enabled = sprite != null;

        lastExpressionIndex = expressionIndex;
    }

    public void SetExpression(int index)
    {
        expressionIndex = Mathf.Clamp(index, 0, 8);
        RefreshPortrait();
    }

    public void SetActor(string name)
    {
        actorName = name;
        cachedActor = null;
        RefreshPortrait();
    }

    public void CycleExpression()
    {
        expressionIndex = (expressionIndex + 1) % 9;
        RefreshPortrait();
    }

    private Sprite GetPortraitSprite(Actor actor, int index)
    {
        if (actor == null) return null;

        // Index 0 = default portrait
        if (index == 0)
        {
            return GetSpriteFromPortrait(actor.portrait);
        }

        // Index 1-8 = alternate portraits (array index 0-7)
        int altIndex = index - 1;

        if (actor.spritePortraits != null && altIndex < actor.spritePortraits.Count)
        {
            return actor.spritePortraits[altIndex];
        }

        if (actor.alternatePortraits != null && altIndex < actor.alternatePortraits.Count)
        {
            return CreateSpriteFromTexture(actor.alternatePortraits[altIndex]);
        }

        return null;
    }

    private Sprite GetSpriteFromPortrait(Texture2D texture)
    {
        if (texture == null) return null;
        return CreateSpriteFromTexture(texture);
    }

    private Sprite CreateSpriteFromTexture(Texture2D texture)
    {
        if (texture == null) return null;
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}

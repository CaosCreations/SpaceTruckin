using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCData npcData;
    public NPCData Data => npcData;

    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        if (Animator == null)
        {
            Debug.LogWarning($"{gameObject.name} NPC has no {nameof(Animator)} reference assigned");
        }

        if (Data == null)
        {
            //Debug.LogWarning($"{gameObject.name} NPC has no {nameof(NPCData)} ScriptableObject reference assigned");
            return;
        }

        if (!Data.LocationByDateContainer.LocationsByDate.IsNullOrEmpty())
        {
            Data.LocationByDateContainer.InitLookup();
        }

        if (!Data.AnimationContextByDateContainer.AnimationContextsByDate.IsNullOrEmpty())
        {
            Data.AnimationContextByDateContainer.InitLookup();
        }
    }

    public override string ToString()
    {
        return $"NPC with name '{gameObject.name}'";
    }
}
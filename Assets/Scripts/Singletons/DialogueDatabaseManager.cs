using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueDatabaseManager : MonoBehaviour
{
    public static DialogueDatabaseManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Increase (or decrease if negative) an actor's Fondness field value, 
    /// i.e. its relationship points.
    /// </summary>
    /// <param name="actorName"></param>
    /// <param name="valueToAdd"></param>
    public static void AddToActorFondness(string actorName, int valueToAdd)
    {
        if (!string.IsNullOrWhiteSpace(actorName))
        {
            // Get the value of the fondness field from the actor in the dialogue database.
            Lua.Result actorFondnessField = DialogueLua.GetActorField(actorName, DialogueConstants.FondnessFieldName);

            if (actorFondnessField.luaValue == null)
            {
                Debug.LogError($"Actor '{actorName}' field '{DialogueConstants.FondnessFieldName} does not exist");
                return;
            }

            int currentFondness = actorFondnessField.asInt;

            // Set the fondness field with the increased value.
            DialogueLua.SetActorField(actorName, DialogueConstants.FondnessFieldName, currentFondness + valueToAdd);
        }
    }
}

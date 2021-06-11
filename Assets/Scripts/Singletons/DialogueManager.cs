using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

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

    public static void IncreaseActorFondess(string actorName, int valueToAdd)
    {

        if (!string.IsNullOrWhiteSpace(actorName))
        {
            // Get the value of the fondness field from the actor in the dialogue database.
            int currentFondness = DialogueLua.GetActorField(actorName, DialogueConstants.FondnessFieldName).asInt;
            int newFondness = currentFondness + valueToAdd;

            // Set the fondness field with the increased value.
            DialogueLua.SetActorField(actorName, DialogueConstants.FondnessFieldName, newFondness);
        }
    }
}

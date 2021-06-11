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

    public static int IncreaseActorFondess(string actorName, int valueToAdd)
    {
        int currentFondness = DialogueLua.GetActorField(actorName, DialogueConstants.FondnessFieldName).asInt;
        int newFondness = currentFondness + valueToAdd;

        DialogueLua.SetActorField(actorName, DialogueConstants.FondnessFieldName, newFondness);
        
        return newFondness;
    }

}

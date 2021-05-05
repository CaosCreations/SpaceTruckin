using UnityEngine;

public class UICanvasBase : MonoBehaviour
{
    [field: SerializeField] public GameObject CanvasTutorialPrefab { get; private set; }
    
    public virtual void ShowTutorial()
    {
        if (CanvasTutorialPrefab != null)
        {
            GameObject tutorial = Instantiate(CanvasTutorialPrefab, transform);

            CardCycle cardCycle = tutorial.GetComponent<CardCycle>();

            if (cardCycle != null)
            {
                cardCycle.SetupCardCycle();
            }
        }
    }
}
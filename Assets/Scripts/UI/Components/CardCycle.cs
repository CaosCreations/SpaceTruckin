using UnityEngine;
using UnityEngine.UI;

public interface ICyclable<T>
{
    T[] CyclableContent { get; set; }
    void Cycle();
}

public class CardCycle : MonoBehaviour, ICyclable<string>
{
    [field: SerializeField] public string[] CyclableContent { get; set; }

    [SerializeField] private Text cardText; 
    [SerializeField] private Button cardCycleButton;

    private int currentIndex;
    private string CurrentCardContent => CyclableContent[currentIndex];
    private bool OnLastCard => currentIndex >= CyclableContent.Length - 1;

    private void OnEnable()
    {
        currentIndex = 0;
        cardText.SetText(CurrentCardContent);
        cardCycleButton.AddOnClick(Cycle);
    }

    public void Cycle()
    {
        currentIndex = (currentIndex + 1) % CyclableContent.Length;
        cardText.SetText(CurrentCardContent);

        if (OnLastCard)
        {
            SetupCloseButton();
        }
    }

    private void SetupCloseButton()
    {
        cardCycleButton.AddOnClick(() => gameObject.SetActive(false));
        cardCycleButton.SetText(UIConstants.CloseCardCycleText);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class CardContainer : MonoBehaviour
{
    private BlackjackPlayer blackjackPlayer;
    private Text totalText;
    public void SetTotalText() 
        => totalText.text = $"{blackjackPlayer.playerName} total: {blackjackPlayer.handTotal}";

    private void Start()
    {
        blackjackPlayer = GetComponent<BlackjackPlayer>();
    }

    public void DestroyCards()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Image>())
            {
                Destroy(child.gameObject);
            }
        }
    }
}

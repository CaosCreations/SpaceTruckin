using UnityEngine;
using UnityEngine.UI;

public class CardContainer : MonoBehaviour
{
    public Text totalText;
    private BlackjackPlayer blackjackPlayer;
    public void SetTotalText() 
        => totalText.text = $"{blackjackPlayer.playerName} total: {blackjackPlayer.handTotal}";

    private void Start()
    {
        blackjackPlayer = GetComponent<BlackjackPlayer>();
    }

    private void Init(GameObject parentObject)
    {

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

using UnityEngine;
using UnityEngine.UI;

public class CardContainer : MonoBehaviour
{
    public Text totalText; // need to assign this during init 
    // make private if poss 

    public void SetTotalText(int handTotal) => totalText.text = handTotal.ToString();
}

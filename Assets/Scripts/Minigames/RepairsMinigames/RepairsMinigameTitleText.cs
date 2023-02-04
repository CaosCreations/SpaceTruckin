using System;
using UnityEngine;
using UnityEngine.UI;

public class RepairsMinigameTitleText : MonoBehaviour
{
    private Text titleText;

    [SerializeField]
    private ShipDamageType damageType;

    private void Awake()
    {
        if (!TryGetComponent(out titleText))
            throw new Exception("No title text found in RepairsMinigameTitleText");

        titleText.text = damageType.ToString() + " Repairs Minigame!";
    }
}
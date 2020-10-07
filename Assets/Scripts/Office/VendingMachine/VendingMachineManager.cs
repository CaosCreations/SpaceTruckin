using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachineManager : MonoBehaviour
{
    // Items indices correspond to the keys on the keypad 
    public VendingMachineItemContainer items;

    // Store corresponding gameObjects to provide purchase feedback later  
    public GameObject[] itemObjects;

    public GameObject itemContainer;
    public GameObject keypadContainer;

    public GameObject itemPrefab;
    public GameObject keyPrefab;

    public Text feedbackText;

    // Hide these later on 
    private Color positiveFeedbackColour = Color.green;
    private Color negativeFeedbackColour = Color.red;

    private void Start()
    {
        itemObjects = new GameObject[items.items.Length];

        InitialiseItems();
        InitialiseKeypad();

        PlayerData.playerMoney = 1000;
    }

    private void InitialiseItems()
    {
        for (int i = 0; i < items.items.Length; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, itemContainer.transform);
            itemObjects[i] = newItem;
            newItem.name = "Item " + i.ToString();

            //Text text = newItem.GetComponentInChildren<Text>();
            //text.text = items.items[i].itemName;

            Image image = newItem.GetComponent<Image>();
            image.sprite = items.items[i].sprite;
        }
    }

    private void InitialiseKeypad()
    {
        AssignKeyCodes();

        foreach (VendingMachineItem item in items.items)
        {
            GameObject key = Instantiate(keyPrefab, keypadContainer.transform);
            key.name = "Key " + item.keyCode;

            Button button = key.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                AddPurchaseListener(item.keyCode);
            });

            Text text = key.GetComponentInChildren<Text>();
            text.text = item.keyCode.ToString();
        }
    }

    private void AssignKeyCodes()
    {
        for (int i = 0; i < items.items.Length; i++)
        {
            Debug.Log(i);
            items.items[i].keyCode = (byte)i;
        }
    }

    private void AddPurchaseListener(int itemIndex)
    {
        Image image = itemObjects[itemIndex].GetComponent<Image>();
        ResetAllColours();

        if (PlayerData.playerMoney >= items.items[itemIndex].price)
        {
            items.items[itemIndex].PurchaseItem();
            image.color = positiveFeedbackColour;
            feedbackText.text = items.items[itemIndex].itemName + " has been purchased!";
        }
        else
        {
            image.color = negativeFeedbackColour;
            feedbackText.text = "Insufficient funds.";
        }
    }

    private void ResetAllColours()
    {
        foreach (GameObject itemObject in itemObjects)
        {
            itemObject.GetComponent<Image>().color = Color.white;
        }
    }
}
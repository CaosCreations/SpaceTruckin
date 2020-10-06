using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachineManager : MonoBehaviour
{
    // Items indices correspond to the keys on the keypad 
    public VendingMachineItemContainer items; 

    // Store corresponding gameObjects to manipulate them later 
    public GameObject[] itemObjects;

    public GameObject itemContainer;
    public GameObject keypadContainer;

    public GameObject itemPrefab;
    public GameObject keyPrefab;

    public Text feedbackText;

    private Color positiveFeedbackColour = Color.green;
    private Color negativeFeedbackColour = Color.red;

    private void Start()
    {
        itemObjects = new GameObject[items.items.Length];

        Debug.Log("Items length: " + items.items.Length);
        InitialiseItems();
        InitialiseKeypad();
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
        for (int i = 0; i < items.items.Length; i++)
        {
            Debug.Log("i value: " + i);

            GameObject key = Instantiate(keyPrefab, keypadContainer.transform);
            key.name = "Key " + i;

            Button button = key.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                Debug.Log("i addlistener: " + i);
                AddPurchaseListener(i);
            });

            Text text = key.GetComponentInChildren<Text>();
            text.text = i.ToString();
        }

        //int itemIndex = 0; 
        //foreach (VendingMachineItem item in items.items)
        //{
        //    GameObject key = Instantiate(keyPrefab, keypadContainer.transform);
        //    key.name = "Key " + itemIndex; 

        //    Button button = key.GetComponent<Button>();
        //    button.onClick.RemoveAllListeners();
        //    button.onClick.AddListener(delegate
        //    {
        //        Debug.Log("i addlistener: " + itemIndex);
        //        AddPurchaseListener(itemIndex);
        //    });

        //    Text text = key.GetComponentInChildren<Text>();
        //    text.text = itemIndex.ToString();

        //    itemIndex++; 
        //}
    }

    private void AddPurchaseListener(int itemIndex)
    {
        Debug.Log("Item index: " + itemIndex);

        Image image = itemObjects[itemIndex].GetComponent<Image>();
        ResetAllColours();

        if (PlayerData.playerMoney >= items.items[itemIndex].price)
        {
            Debug.Log(items.items[itemIndex] + " has been purchased");
            items.items[itemIndex].PurchaseItem();
            image.color = positiveFeedbackColour;
        }
        else
        {
            image.color = negativeFeedbackColour;
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

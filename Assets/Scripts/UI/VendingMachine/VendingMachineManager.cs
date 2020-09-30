using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachineManager : MonoBehaviour
{
    // Items indices correspond to the keys on the keypad 
    public VendingMachineItem[] items;

    public GameObject[] itemObjects;
    //public List<GameObject> itemObjects;

    public GameObject itemContainer;
    public GameObject keypadContainer;

    public GameObject itemPrefab;
    public GameObject keyPrefab;

    public Text feedbackText;

    private int numberOfKeys = 10;
    private Color positiveFeedbackColour = Color.green;
    private Color negativeFeedbackColour = Color.red;

    private void Start()
    {
        items = new VendingMachineItem[numberOfKeys];
        //itemObjects = new List<GameObject>(); 

        Debug.Log("Items length: " + items.Length);
        InitialiseItems();
        InitialiseKeypad();
    }


    private void InitialiseItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            GameObject item = Instantiate(itemPrefab, itemContainer.transform);
            itemObjects[i] = item;
            item.name = "Item " + i.ToString();

            Text text = item.GetComponent<Text>();
            text.text = items[i].itemName;

            Image image = GetComponent<Image>();
            image.sprite = items[i].sprite;
        }
    }

    private void InitialiseKeypad()
    {
        for (int i = 0; i < items.Length; i++)
        {
            GameObject key = Instantiate(keyPrefab, keypadContainer.transform);
            key.name = "Key " + i.ToString();

            Text text = key.GetComponentInChildren<Text>();
            text.text = i.ToString();

            Button button = key.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                AddPurchaseListener(i);
            });
        }
    }

    private void AddPurchaseListener(int itemIndex)
    {
        Image image = itemObjects[itemIndex].GetComponent<Image>();

        if (PlayerData.playerMoney >= items[itemIndex].price)
        {
            Debug.Log(items[itemIndex] + " has been purchased");
            items[itemIndex].PurchaseItem();
            image.color = positiveFeedbackColour;
        }
        else
        {
            image.color = negativeFeedbackColour;
        }
    }

}

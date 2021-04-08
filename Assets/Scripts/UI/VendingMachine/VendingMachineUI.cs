using UnityEngine;
using UnityEngine.UI;

public class VendingMachineUI : UICanvasBase
{
    // Items indices correspond to the keys on the keypad 
    public VendingMachineItemContainer vendingMachineItemContainer;
    private VendingMachineItem[] Items { get => vendingMachineItemContainer.items; }

    public GameObject itemContainer;
    public GameObject keypadContainer;

    public GameObject itemPrefab;
    public GameObject keyPrefab;

    public Text feedbackText;

    // Store corresponding gameObjects to provide purchase feedback  
    private GameObject[] itemObjects;

    // Hide these later on 
    private Color positiveFeedbackColour = Color.green;
    private Color negativeFeedbackColour = Color.red;

    private void Start()
    {
        itemObjects = new GameObject[Items.Length];

        InitialiseItems();
        InitialiseKeypad();
    }

    private void InitialiseItems()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, itemContainer.transform);
            itemObjects[i] = newItem;
            newItem.name = "Item " + i.ToString();
            newItem.SetSprite(Items[i].sprite);
        }
    }

    private void InitialiseKeypad()
    {
        AssignKeyCodes();

        foreach (VendingMachineItem item in Items)
        {
            GameObject key = Instantiate(keyPrefab, keypadContainer.transform);
            key.name = "Key " + item.keyCode;
            Button keyButton = key.GetComponent<Button>();
            keyButton.AddOnClick(() => AddPurchaseListener(item.keyCode)).SetText(item.keyCode.ToString());
        }
    }

    private void AssignKeyCodes()
    {
        for (byte i = 0; i < Items.Length; i++)
        {
            Items[i].keyCode = i;
        }
    }

    private void AddPurchaseListener(int itemIndex)
    {
        Image image = itemObjects[itemIndex].GetComponent<Image>();
        ResetAllColours();

        if (PlayerManager.Instance.CanSpendMoney(Items[itemIndex].price))
        {
            Items[itemIndex].PurchaseItem();
            image.color = positiveFeedbackColour;
            feedbackText.SetText(Items[itemIndex].itemName + " has been purchased!");
        }
        else
        {
            image.color = negativeFeedbackColour;
            feedbackText.SetText("Insufficient funds.");
        }
    }

    private void OnDisable()
    {
        CleanUI();
    }

    private void ResetAllColours()
    {
        foreach (GameObject itemObject in itemObjects)
        {
            itemObject.GetComponent<Image>().color = Color.white;
        }
    }
    
    public void CleanUI()
    {
        feedbackText.Clear();
        ResetAllColours();
    }
}
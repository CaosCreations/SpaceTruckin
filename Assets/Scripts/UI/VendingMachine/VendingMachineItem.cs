using UnityEngine;

[CreateAssetMenu(fileName = "VendingMachineItem", menuName = "ScriptableObjects/VendingMachineItem", order = 1)]
public class VendingMachineItem : ScriptableObject
{
    public string itemName;
    public int price;
    public int stock;
    public int keyCode;

    public Sprite sprite;

    public void PurchaseItem()
    {
        PlayerManager.Instance.SpendMoney(price); 
    }
}

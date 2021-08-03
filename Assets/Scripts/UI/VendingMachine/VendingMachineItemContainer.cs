using UnityEngine;

[CreateAssetMenu(fileName = "VendingMachineItemContainer", menuName = "ScriptableObjects/VendingMachineItemContainer", order = 1)]
public class VendingMachineItemContainer : ScriptableObject, IScriptableObjectContainer<VendingMachineItem>
{
    public VendingMachineItem[] Elements { get; set; }
}

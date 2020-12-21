using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VendingMachineItemContainer", menuName = "ScriptableObjects/VendingMachineItemContainer", order = 1)]
public class VendingMachineItemContainer : ScriptableObject
{
    public VendingMachineItem[] items;
}
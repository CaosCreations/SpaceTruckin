using System;
using UnityEngine;

[RequireComponent(typeof(BatteryCharging))]
[RequireComponent(typeof(BatteryInteractable))]
public class BatteryWrapper : MonoBehaviour
{
    public BatteryInteractable BatteryInteractable;
    public BatteryCharging BatteryCharging;

    #region Persistence
    public const string FOLDER_NAME = "HangarSaveData";
    public const string FILE_NAME = "BatterySaveData";
    public static string FILE_PATH
    {
        get => DataUtils.GetSaveFilePath(FOLDER_NAME, FILE_NAME);
    }
    #endregion
}

[Serializable]
public struct BatterySaveData
{
    public bool IsCharged;
    public Vector3 PositionInHangar;
}

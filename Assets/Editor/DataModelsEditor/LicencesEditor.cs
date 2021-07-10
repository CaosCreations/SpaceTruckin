﻿using System;
using UnityEditor;
using UnityEngine;

public class LicencesEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Licences/Own All Licences")]
    private static void OwnAllLicences() => SetAllIsOwned(true);

    [MenuItem("Space Truckin/Licences/Disown All Licences")]
    private static void DisownAllLicences() => SetAllIsOwned(false);

    [MenuItem("Space Truckin/Licences/Give 100 LP")]
    private static void Give100LP() => GiveLP(100);

    private static void SetAllIsOwned(bool isOwned)
    {
        try
        {
            var licenceContainer = EditorHelper.GetAsset<LicenceContainer>();

            foreach (var licence in licenceContainer.Licences)
            {
                licence.IsOwned = isOwned;
            }

            EditorUtility.SetDirty(licenceContainer);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private static void GiveLP(int points)
    {
        try
        {
            var playerData = EditorHelper.GetAsset<PlayerData>();
            playerData.PlayerLicencePoints += points;
            playerData.PlayerTotalLicencePointsAcquired += points;
            EditorUtility.SetDirty(playerData);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void DeleteSaveData()
    {
        var licenceContainer = EditorHelper.GetAsset<LicenceContainer>();
        foreach (var licence in licenceContainer.Licences)
        {
            SaveDataEditor.NullifyFields(licence.saveData);
        }
    }
}
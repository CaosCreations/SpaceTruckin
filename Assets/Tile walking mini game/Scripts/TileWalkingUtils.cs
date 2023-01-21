using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWalkingUtils
{
   public static HashSet<int> GetRandomNonRepeatingNumbers(int outputSize, int minNumber, int maxNumber)
    {
        System.Random random = new System.Random();

        HashSet<int> randomTileIndices = new HashSet<int>();

        while (randomTileIndices.Count < outputSize)
        {
            randomTileIndices.Add(random.Next(minNumber, maxNumber));
        }

        foreach(int item in randomTileIndices)
        {
            Debug.Log(item);
        }

        return randomTileIndices;
    }
}

using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public int bodyIndex;
    public int hairIndex;
    public int clothesIndex;

    public CharacterData(int body, int hair, int clothes)
    {
        bodyIndex = body;
        hairIndex = hair;
        clothesIndex = clothes;
    }
}

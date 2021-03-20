using System;
using UnityEngine;

// Todo: amalgamate this with DataUtils
public static class JsonHelper
{
    public static T[] ArrayFromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ArrayToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        string json = JsonUtility.ToJson(wrapper);
        return json;
    }

    public static string ArrayToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>
        {
            Items = array
        };
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        // Used to wrap the array of objects 
        public T[] Items;
    }
}

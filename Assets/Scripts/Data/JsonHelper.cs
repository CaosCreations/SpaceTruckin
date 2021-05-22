using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static string ArrayToJson<T>(T[] array)
    {
        ArrayWrapper<T> wrapper = new ArrayWrapper<T>();
        wrapper.Items = array;
        string json = JsonUtility.ToJson(wrapper);
        return json;
    }

    public static T[] ArrayFromJson<T>(string json)
    {
        ArrayWrapper<T> wrapper = JsonUtility.FromJson<ArrayWrapper<T>>(json);
        return wrapper?.Items;
    }

    public static string ListToJson<T>(List<T> list)
    {
        ListWrapper<T> wrapper = new ListWrapper<T>
        {
            Items = list
        };
        return JsonUtility.ToJson(wrapper);
    }

    public static List<T> ListFromJson<T>(string json)
    {
        ListWrapper<T> wrapper = JsonUtility.FromJson<ListWrapper<T>>(json);
        return wrapper?.Items;
    }

    [Serializable]
    private class ArrayWrapper<T>
    {
        // Used to wrap the array of objects 
        public T[] Items;
    }

    [Serializable]
    private class ListWrapper<T>
    {
        // Used to wrap the list of objects 
        public List<T> Items;
    }
}

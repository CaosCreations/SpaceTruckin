using UnityEngine;

public interface IScriptableObjectContainer<T> where T : ScriptableObject
{
    T[] Elements { get; set; }
}

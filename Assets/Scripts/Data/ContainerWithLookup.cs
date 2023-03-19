using System.Collections.Generic;

public abstract class ContainerWithLookup<TKey, TValue>
{
    public Dictionary<TKey, TValue> Lookup { get; protected set; }

    public abstract void InitLookup();
}
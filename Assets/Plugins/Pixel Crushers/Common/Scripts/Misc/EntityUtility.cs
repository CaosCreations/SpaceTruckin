// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers
{

    /// <summary>
    /// Wrapper for object/entity IDs to handle API differences in Unity versions.
    /// </summary>
    [System.Serializable]
    public struct EntityIdWrapper
    {
        public bool isValid;
#if UNITY_6000_3_OR_NEWER
        public EntityId id;
        public EntityIdWrapper(EntityId id) { this.id = id; isValid = true; }
        public int intValue => id.GetHashCode();
        public static EntityIdWrapper None = new EntityIdWrapper(EntityId.None) { isValid = false };
#else
        public int id;
        public EntityIdWrapper(int id) { this.id = id; isValid = true; }
        public int intValue => id;
        public static EntityIdWrapper None = new EntityIdWrapper(-1) { isValid = false };
#endif
    }

    /// <summary>
    /// Utility functions for working with objects/entities.
    /// </summary>
    public static class EntityUtility
    {

       public static EntityIdWrapper GetEntityId(UnityEngine.Object obj)
        {

            if (obj == null) return new EntityIdWrapper();
#if UNITY_6000_3_OR_NEWER
            return new EntityIdWrapper(obj.GetEntityId());
#else
            return new EntityIdWrapper(obj.GetInstanceID());
#endif
        }

        public static int GetEntityInt(UnityEngine.Object obj)
        {
            return GetEntityId(obj).intValue;
        }

    }

}
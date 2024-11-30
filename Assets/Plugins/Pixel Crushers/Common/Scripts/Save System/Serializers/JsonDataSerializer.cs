// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers
{

    /// <summary>
    /// Implementation of DataSerializer that uses JsonUtility.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper instead.
    public class JsonDataSerializer : DataSerializer
    {

        [Tooltip("Use larger but more human-readable format.")]
        [SerializeField]
        private bool m_prettyPrint;

        public bool prettyPrint
        {
            get { return m_prettyPrint; }
            set { m_prettyPrint = value; }
        }

        public override string Serialize(object data)
        {
            return JsonUtility.ToJson(data, m_prettyPrint);
        }

        public override T Deserialize<T>(string s, T data = default(T))
        {
            if (Equals(data, default(T)))
            {
                return JsonUtility.FromJson<T>(s);
            }
            else
            {
                JsonUtility.FromJsonOverwrite(s, data);
                return data;
            }
        }

    }

}

using UnityEngine;


namespace Assets.Scripts.Singletons
{
    public class PilotsManager : MonoBehaviour
    {
        public static PilotsManager Instance;

        public Pilot[] pilots; 

        private void Awake()
        {
            if (FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }

            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(Instance.gameObject);
                Instance = this;
            }
            DontDestroyOnLoad(this.gameObject);
        }
    }
}

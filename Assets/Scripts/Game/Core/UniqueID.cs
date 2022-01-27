using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Entity))]
    public class UniqueID : MonoBehaviour
    {
        [SerializeField] private string uniqueID;

        private static Dictionary<string, GameObject> uniqueObjects = new Dictionary<string, GameObject>();

        protected void Awake()
        {
            if (!ValidateNotEmpty(uniqueID)) return;
            if (!ValidateExists(uniqueID)) return;
            uniqueObjects.Add(uniqueID, gameObject);
        }

        ///<summary>Low-level interface for getting objects by unique ID.</summary>
        public static GameObject Get(string id)
        {
            GameObject obj = uniqueObjects.ContainsKey(id) ? uniqueObjects[id] : null;
            if (obj == null)
            {
                Debug.LogWarning($"UniqueID.Get() failed to find Object with ID \"{id}\"; returning null.");
                return null;
            }
            return obj;
        }

        ///<summary>Low-level generic interface for getting objects by unique ID.</summary>
        public static T Get<T>(string id) where T : Object
        {
            GameObject obj = Get(id);
            return obj != null ? obj.GetComponent<T>() : null;
        }

        private bool ValidateNotEmpty(string uniqueID)
        {
            if (uniqueID == "")
            {
                Debug.LogWarning($"UniqueID is empty on object {name}!");
                return false;
            }
            return true;
        }

        private bool ValidateExists(string uniqueID)
        {
            if (uniqueObjects.ContainsKey(uniqueID))
            {
                Debug.LogError($"ERROR: UniqueID \"{uniqueID}\" on object {name} already exists! It belongs to {Get(uniqueID).name}.");
                return false;
            }
            return true;
        }
    }
}

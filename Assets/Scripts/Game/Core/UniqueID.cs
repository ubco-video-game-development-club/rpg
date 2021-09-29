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

        protected void OnValidate()
        {
            if (uniqueID == null) return;
            if (!ValidateNotEmpty(uniqueID)) return;
            if (uniqueObjects.ContainsKey(uniqueID) && uniqueObjects[uniqueID] == gameObject) return;
            if (!ValidateExists(uniqueID)) return;
        }

        protected void Awake()
        {
            if (!ValidateNotEmpty(uniqueID)) return;
            if (!ValidateExists(uniqueID)) return;
            uniqueObjects.Add(uniqueID, gameObject);
        }

        ///<summary>Low-level interface for getting objects by unique ID. You almost always want the Entity.Find() wrapper instead!</summary>
        public static GameObject Get(string id)
        {
            return uniqueObjects.ContainsKey(id) ? uniqueObjects[id] : null;
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

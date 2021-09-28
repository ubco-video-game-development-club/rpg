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
            if (uniqueObjects.ContainsKey(uniqueID))
            {
                Debug.LogError($"ERROR: UniqueID \"{uniqueID}\" on object {name} already exists! It belongs to {Get(uniqueID).name}.");
                return;
            }
            uniqueObjects.Add(uniqueID, gameObject);
        }

        ///<summary>Low-level interface for getting objects by unique ID. You almost always want the Entity.Find() wrapper instead!</summary>
        public static GameObject Get(string uniqueID)
        {
            return uniqueObjects.ContainsKey(uniqueID) ? uniqueObjects[uniqueID] : null;
        }
    }
}

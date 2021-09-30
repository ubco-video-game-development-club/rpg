using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueIndexOverride
    {
        [Tooltip("The UniqueID of the target entity. Leave blank to target self.")]
        public string targetUniqueID;
        [Tooltip("The dialogue graph index to set active on the target entity.")]
        public int indexOverride;
    }
}

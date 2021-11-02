using UnityEngine;

namespace RPG
{
    public interface IInstantiable<T> where T : ScriptableObject
    {
        T GetInstance();
    }
}

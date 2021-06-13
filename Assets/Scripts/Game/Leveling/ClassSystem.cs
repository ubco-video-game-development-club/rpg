using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassEditor;

namespace RPG
{
    public class ClassSystem : MonoBehaviour
    {
        [SerializeField] private ClassTree myClass;

        void Start()
        {
            SelectClass(myClass);
        }

        public void SelectClass(ClassTree classTree)
        {
            // TODO: save the class to the player?
            ClassData data = classTree.GetClassData();
            GameManager.Player.SetProperty<int>(PropertyName.MaxHealth, data.health);
            GameManager.Player.SetProperty<int>(PropertyName.Health, data.health);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassEditor;

namespace RPG
{
    public class ClassSelectMenu : MonoBehaviour
    {
        [SerializeField] private ClassTree defaultTree;

        private void Start()
        {
            SetClassChoice(defaultTree);
        }

        public void SetClassChoice(ClassTree classTree)
        {
            GameManager.ClassSystem.SelectClass(classTree);
        }
    }
}

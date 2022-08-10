using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] private string sceneName = "Main";

        public void LoadScene()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

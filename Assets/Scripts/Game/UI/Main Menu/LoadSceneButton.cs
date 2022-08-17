using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] private string sceneName = "Main";
        [SerializeField] private float minLoadTime = 2f;

        public void LoadScene()
        {
            StartCoroutine(LoadSceneAsync());
        }

        private IEnumerator LoadSceneAsync()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
            loadOperation.allowSceneActivation = false;

            float loadTimer = 0f;

            while (!loadOperation.isDone)
            {
                if (loadTimer >= minLoadTime && loadOperation.progress >= 0.9f)
                {
                    loadOperation.allowSceneActivation = true;
                }

                loadTimer += Time.deltaTime;
                yield return null;
            }

            Debug.Log("YOOOOO");
        }
    }
}

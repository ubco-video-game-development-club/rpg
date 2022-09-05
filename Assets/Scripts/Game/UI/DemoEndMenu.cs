using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class DemoEndMenu : Menu
    {
        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        public void SetDemoEnd()
        {
            enabled = true;
        }
    }
}

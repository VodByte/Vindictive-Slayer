using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour 
{
    enum Menu
    {
        Resume,
        Quit
    }
    Menu currentMenu = Menu.Quit;

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }

    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            currentMenu = currentMenu == Menu.Resume ? Menu.Quit : Menu.Resume;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentMenu == Menu.Quit) Application.Quit();
            else gameObject.SetActive(false);
        }
    }
}

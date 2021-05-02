using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Loader : MonoBehaviour
{
    public void Play_Button()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit_Game()
    {
        Application.Quit();
    }
}

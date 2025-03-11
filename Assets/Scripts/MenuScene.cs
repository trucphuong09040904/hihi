using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene(0);
    }

    public void book()
    {
        SceneManager.LoadScene(2);
    }

    public void back()
    {
        SceneManager.LoadScene(1);
    }
}

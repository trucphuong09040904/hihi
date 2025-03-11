using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CHANGEMENUMOI : MonoBehaviour
{
    public void BTNPLAY()
    {
        SceneManager.LoadScene(0);
    }
    public void BTNSTORY()
    {
        SceneManager.LoadScene(6);
    }
    public void BTNQUIT()
    {
        Application.Quit();
    }
    public void BTNQUITSTORY()
    {
        SceneManager.LoadScene(4);
    }
    public void BTNSETTING()
    {
        SceneManager.LoadScene(5);
    }
    public void BTNQUITSETTING()
    {
        SceneManager.LoadScene(4);
    }
}
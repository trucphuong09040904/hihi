using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CHANGEMENUMOI : MonoBehaviour
{
    public void BTNPLAY()
    {
        // Ki?m tra xem ng??i ch?i ?ã ch?n m?c ?? khó nào tr??c ?ó
        int difficulty = PlayerPrefs.GetInt("Difficulty", 0); // M?c ??nh là 0 (D?)

        if (difficulty == 1)
        {
            SceneManager.LoadScene(0); // Ch? ?? Medium -> Scene 0
        }
        else if (difficulty == 2)
        {
            SceneManager.LoadScene(7); // Ch? ?? Hard -> Scene 8
        }
        else
        {
            SceneManager.LoadScene(0); // M?c ??nh n?u ch?a ch?n gì
        }
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

    public void BTNMEDIUM()
    {
        PlayerPrefs.SetInt("Difficulty", 1); // L?u ch? ?? Medium
        PlayerPrefs.Save(); // L?u l?i d? li?u
    }

    public void BTNHARD()
    {
        PlayerPrefs.SetInt("Difficulty", 2); // L?u ch? ?? Hard
        PlayerPrefs.Save(); // L?u l?i d? li?u
    }
}

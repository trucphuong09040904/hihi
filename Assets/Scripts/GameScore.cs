using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Thêm thư viện TMP

public class GameScore : MonoBehaviour
{

    public static GameScore instance; // 🔹 Biến instance để gọi từ bất kỳ đâu




    TMP_Text scoreTextUI; // Sử dụng TMP_Text thay vì Text
    int score;

    public int targetScore1 = 1000;
    public int targetScore2 = 2000;
    public int targetScore3 = 3000;
    public int targetScore4 = 4000;
    public int targetScore5 = 5000;

    public string scene1 = "PlayScene";
    public string scene2 = "PlayScene 1";
    public string scene3 = "PlayScene 2";
    public string scene4 = "PlayScene 3";
    public string scene5 = "PlayScene 4";

    public int Score
    {
        get { return this.score; }
        set
        {
            this.score = value;
            UpdateScoreTextUI();
            CheckAndLoadNextScene();
        }
    }

    void Awake()
    {
        // 🔹 Đảm bảo chỉ có 1 instance của GameScore
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        scoreTextUI = GameObject.Find("ScoreText").GetComponent<TMP_Text>();

        if (scoreTextUI == null)
        {
            Debug.LogError("GameScore: Không tìm thấy TMP Text UI! Hãy đảm bảo có TextMeshPro trong Canvas.");
        }
        else
        {
            UpdateScoreTextUI();
        }
    }

    void UpdateScoreTextUI()
    {
        if (scoreTextUI != null)
        {
            scoreTextUI.text = string.Format("{0:000000}", score);
        }
    }

    void CheckAndLoadNextScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == scene1 && score >= targetScore1)
        {
            LoadNextScene(scene2);
        }
        else if (currentScene == scene2 && score >= targetScore2)
        {
            LoadNextScene(scene3);
        }
        else if (currentScene == scene3 && score >= targetScore3)
        {
            LoadNextScene(scene4);
        }
        else if (currentScene == scene4 && score >= targetScore4)
        {
            LoadNextScene(scene5);
        }
    }

    void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    // 🔹 Thêm hàm này để cập nhật điểm số khi Boss bị bắn
    public void AddScore(int amount)
    {
        Score += amount;
    }
}
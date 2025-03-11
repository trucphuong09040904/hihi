using UnityEngine;
using TMPro;
using System.Collections;

public class Demgio : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public GameObject gameOverUI;
    public GameObject countdownPanel;
    public TextMeshProUGUI countdownText;

    private float timeRemaining = 91f;
    private bool isGameOver = false;

    void Start()
    {
        Time.timeScale = 0f; // Dừng game ngay từ đầu
        UpdateTimeText();
        StartCoroutine(CountdownBeforeStart());
    }

    IEnumerator CountdownBeforeStart()
    {
        Time.timeScale = 0f; // Đảm bảo game không chạy khi đếm ngược
        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null)
            {
                countdownText.text = i.ToString();
            }
            yield return new WaitForSecondsRealtime(1f);
        }

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }

        Time.timeScale = 1f; // Bắt đầu game sau đếm ngược
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimeText();
            }
            else
            {
                GameOver();
            }
        }
    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        isGameOver = true;
        timeText.text = "00:00";
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        StartCoroutine(PauseGameAfterDelay());
    }

    IEnumerator PauseGameAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }
}
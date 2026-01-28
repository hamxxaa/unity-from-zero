using UnityEngine;
using TMPro; // TextMeshPro kütüphanesi şart!
using System.Collections;

public class GameUI : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] TextMeshProUGUI centerText;
    [SerializeField] TMPro.TextMeshProUGUI scoreText;


    [Header("Panel References")]
    [SerializeField] GameObject gameOverPanel;


    public void UpdateWaveText(int waveIndex)
    {
        waveText.text = "WAVE: " + waveIndex;
    }

    public void UpdateEnemyCount(int count)
    {
        enemyText.text = "ENEMIES: " + count;
    }

    public void ShowWaveAnnouncement(int waveIndex)
    {
        StartCoroutine(AnnouncementRoutine(waveIndex));
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void UpdateScoreUI(int score, int highScore)
    {
        scoreText.text = "Score: " + score + "\nHigh: " + highScore;
    }

    IEnumerator AnnouncementRoutine(int waveIndex)
    {
        centerText.text = "WAVE " + waveIndex;
        centerText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        centerText.gameObject.SetActive(false);
    }
}
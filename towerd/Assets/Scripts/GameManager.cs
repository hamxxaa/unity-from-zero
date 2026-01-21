using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Start, Playing, GameOver, Pause }
    public GameState currentState;

    [Header("UI Panels")]
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject pausePanel;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SwitchState(GameState.Start);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (currentState == GameState.Playing)
            {
                SwitchState(GameState.Pause);
            }
            else if (currentState == GameState.Pause)
            {
                SwitchState(GameState.Playing);
            }
        }
    }

    public void SwitchState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Start:
                Time.timeScale = 0f; // freeze time 
                startPanel.SetActive(true);
                gameOverPanel.SetActive(false);
                pausePanel.SetActive(false);
                break;

            case GameState.Playing:
                Time.timeScale = 1f; // unfreeze time
                startPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                pausePanel.SetActive(false);
                break;

            case GameState.GameOver:
                Time.timeScale = 0f; // freeze time
                startPanel.SetActive(false);
                gameOverPanel.SetActive(true);
                pausePanel.SetActive(false);
                break;

            case GameState.Pause:
                Time.timeScale = 0f; // freeze time
                startPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                pausePanel.SetActive(true);
                break;
        }
    }


    public void OnPlayButtonPressed()
    {
        SwitchState(GameState.Playing);
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void TriggerGameOver()
    {
        SwitchState(GameState.GameOver);
    }
}
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen, deathEffectPlayer, pauseButton;
    [SerializeField] GameObject askDevicePosition, howToPlayScreen;
    [SerializeField] Material playerMaterial, enemyMaterial;

    public static int score, gamesPlayed = 0;

    bool gameOver, gameStarted;
    GameObject player;
    SpawnManager spawner;

	void Start ()
	{
        gameOver = gameStarted = false;
        score = 0;

        player = GameObject.FindGameObjectWithTag("Player");
        spawner = GetComponent<SpawnManager>();
	}

    public void StartGame()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;  // stop the screen from turning off while playing

        if (PlayerPrefs.GetInt("IsFirstTime", 1) == 1)
            FirstTimePlayingInitialize();
        else
        {
            gameStarted = true;
            if (!player.activeInHierarchy)
                player.SetActive(true);
            spawner.StartSpawning();
            ChangeColors();
        }
    }

    void FirstTimePlayingInitialize()
    {
        DifficultyManager.difficulty = (int) Difficulty.EASY;
        PlayerPrefs.SetInt("IsFirstTime", 0);
        howToPlayScreen.SetActive(true);
        askDevicePosition.SetActive(true);
    }

    void ChangeColors()
    {
        SetBackground();
        SetPlayerColor();
        SetEnemyColor();
    }

    void SetBackground()
    {
        Color[] floorColors = { new Color(56f / 255f, 64f / 255f, 89f / 255f), new Color(51f / 255f, 37f / 255f, 50f / 255f), new Color(35f / 255f, 35f / 255f, 35f / 255f) };
        Material floor = GameObject.Find("Floor").GetComponent<Renderer>().material;
        floor.color = floorColors[DifficultyManager.difficulty];
    }

    void SetPlayerColor()
    {
        Color[] playerColors = { new Color(241f / 255f, 222f / 255f, 152f / 255f), new Color(164f / 255f, 154f / 255f, 135f / 255f), new Color(148f / 255f, 90f / 255f, 76f / 255f) };
        playerMaterial.color = playerColors[DifficultyManager.difficulty];
    }

    void SetEnemyColor()
    {
        Color[] enemyColors = { new Color(247f / 255f, 122f / 255f, 82f / 255f), new Color(64f / 255f, 102f / 255f, 98f / 255f), new Color(145f / 255f, 105f / 255f, 91f / 255f) };
        enemyMaterial.color = enemyColors[DifficultyManager.difficulty];
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("main");
    }

    public void setGameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            spawner.StopSpawning();
            finishGame();
        }
    }

    public bool isGameStarted()
    {
        return gameStarted;
    }

    public bool isGameOver()
    {
        return gameOver;
    }

    void finishGame()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        Instantiate(deathEffectPlayer, player.transform.position, player.transform.rotation);

        player.SetActive(false);
        pauseButton.SetActive(false);
        StartCoroutine(ActivateGameOverScreen());
    }

    void updateHighScore()
    {
        string[] highScoreArray = { "EasyHighScore", "MediumHighScore", "HardHighScore" };

        if (score > PlayerPrefs.GetInt(highScoreArray[DifficultyManager.difficulty], 0))
            PlayerPrefs.SetInt(highScoreArray[DifficultyManager.difficulty], score);
        
        GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Best: "
            + PlayerPrefs.GetInt(highScoreArray[DifficultyManager.difficulty], 0);
    }

    IEnumerator ActivateGameOverScreen()
    {
        yield return new WaitForSeconds(1f);
        gameOverScreen.SetActive(true);
        updateHighScore();
        DisplayUserPerformance();
    }

    void DisplayUserPerformance()
    {
        GameObject.Find("ScoreEarned").GetComponent<TextMeshProUGUI>().text = score.ToString();
        GameObject.Find("TimeSurvived").GetComponent<TextMeshProUGUI>().text = TimeElapsed.GetTimeString();
    }
}
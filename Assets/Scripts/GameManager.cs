using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// refactor this code, its a very messy code
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen, deathEffectPlayer, pauseButton;
    [SerializeField] Text scoreText;

    public static int score;

    bool gameOver, gameStarted, spawnedDeathEffect;

    GameObject player;
    SpawnManager spawnManager;

	void Start ()
	{
        gameOver = gameStarted = spawnedDeathEffect = false;
        score = 0;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;  // stop the screen from turning off while playing
        player = GameObject.FindGameObjectWithTag("Player");
        spawnManager = GetComponent<SpawnManager>();

        //player.SetActive(false); error is caused because of this line in the spawnmanager class
	}

	void Update ()
    {
        scoreText.text = "Score: " + score;

        if (Input.touchCount > 0)
            pauseButton.GetComponent<SumPause>().TogglePause();

        if (gameOver)
            finishGame();
	}

    void finishGame()
    {
        gameOverScreen.SetActive(true);
        pauseButton.SetActive(false);

        GameObject.Find("ScoreEarned").GetComponent<Text>().text = "Score: " + score;
       
        if(!spawnedDeathEffect)
        {
            spawnedDeathEffect = true;
            Instantiate(deathEffectPlayer, player.transform.position, player.transform.rotation);
        }
        player.SetActive(false);
    }

    public void StartGame()
    {
        gameStarted = true;
        if(!player.activeInHierarchy)
            player.SetActive(true);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("main");
    }

    public void setGameOver()
    {
        gameOver = true;
    }

    public bool isGameStarted()
    {
        return gameStarted;
    }

    public bool isGameOver()
    {
        return gameOver;
    }
}
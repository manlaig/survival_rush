using UnityEngine;
using TMPro;

public class TimeElapsed : MonoBehaviour
{
    public TextMeshProUGUI score;
    TextMeshProUGUI timeText;
    GameManager gameController;
    public static int initialTime;
    bool timeSet;

    void Start ()
    {
        initialTime = 0;
        timeSet = false;
        timeText = GetComponent<TextMeshProUGUI>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	void Update ()
    {
        if (gameController.isGameStarted() && !timeSet)
        {
            timeSet = true;
            initialTime = (int) Time.time;
            SetRepeatingCalls();
        }
        if (gameController.isGameOver())
        {
            DisableTexts();
            CancelInvoke("DecreaseSpawnDelay");
            CancelInvoke("IncreaseWeaponAllowed");
        }

        if(score.gameObject.activeInHierarchy)
            score.text = "Score: " + GameManager.score;
        UpdateTime();
	}

    void SetRepeatingCalls()
    {
        int[] decreaseDelays = { 15, 13, 11 };
        int[] increaseDelays = { 70, 50, 30 };
        // there was a performance issue involved when the second parameter of invokerepeating is 0f, look into it later
        InvokeRepeating("DecreaseSpawnDelay", 0f, decreaseDelays[DifficultyManager.difficulty]);
        InvokeRepeating("IncreaseWeaponAllowed", 30f, increaseDelays[DifficultyManager.difficulty]);
    }

    void DisableTexts()
    {
        timeText.gameObject.SetActive(false);
        score.gameObject.SetActive(false);
    }

    void UpdateTime()
    {
        if (timeSet && !gameController.isGameOver() && timeText.gameObject.activeInHierarchy)
            timeText.text = "Time: " + GetTimeString();
    }

    public static string GetTimeString()
    {
        int minutes = (int)(Time.time - initialTime) / 60;
        int seconds = (int)(Time.time - initialTime) % 60;
        string mid = (seconds < 10) ? ":0" : ":";
        return minutes.ToString() + mid + seconds.ToString();
    }

    void DecreaseSpawnDelay()
    {
        gameController.gameObject.GetComponent<SpawnManager>().DecreaseSpawnDelay();
    }

    void IncreaseWeaponAllowed()
    {
        gameController.gameObject.GetComponent<SpawnManager>().IncreaseWeaponsAllowed();
    }
}

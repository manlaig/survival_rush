using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Changes the time elapsed text as the game plays
/// </summary>
public class TimeElapsed : MonoBehaviour
{
    Text text;
    GameManager gameController;
    int timeElapsed = 0;
    bool timeSet = false;

    void Start ()
    {
        text = GetComponent<Text>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	void Update ()
    {
        if (gameController.isGameStarted() && !timeSet)
        {
            timeElapsed = (int)Time.time;
            timeSet = true;
        }
        UpdateTime();
	}

    void UpdateTime()
    {
        if (timeSet && !gameController.isGameOver())
        {
            int minutes = (int)((Time.time - timeElapsed) / 60);
            int seconds = (int)((Time.time - timeElapsed) % 60);
            string mid = (seconds < 10) ? ":0" : ":";
            text.text = "Time: " + minutes + mid + seconds;
        }
    }  
}

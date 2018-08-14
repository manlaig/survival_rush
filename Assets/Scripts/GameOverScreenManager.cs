using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenManager : MonoBehaviour
{
	public Image[] stars;
    
	void OnEnable()
    {      
		int[,] performanceRanges = new int[,] { {10000, 25000, 45000},
			{7500, 20000, 37500},
			{5000, 15000, 30000} };
		if(GameManager.score > performanceRanges[DifficultyManager.difficulty, 0])
		{
			Debug.Log("Score is " + GameManager.score);
			stars[0].color = new Color(241f / 255f, 239f/255f, 96f/255f);
		}
		if(GameManager.score > performanceRanges[DifficultyManager.difficulty, 1])
			stars[1].color = new Color(241f / 255f, 239f/255f, 96f/255f);
		if(GameManager.score > performanceRanges[DifficultyManager.difficulty, 2])
            stars[2].color = new Color(241f / 255f, 239f/255f, 96f/255f);
    }
}

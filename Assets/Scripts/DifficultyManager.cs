using UnityEngine;
using UnityEngine.UI;

public enum Difficulty
{
    EASY = 0, MEDIUM = 1, HARD = 2
}

public class DifficultyManager : MonoBehaviour
{
    public static int difficulty;
    Button[] difficultyButtons;

    void Start()
    {
        difficulty = (int) Difficulty.MEDIUM;
        difficultyButtons = GetComponentsInChildren<Button>();
    }

    public void SetDifficulty(int index)
    {
        toggleSelected(true);
        difficulty = index;
        toggleSelected(false);
    }

    void toggleSelected(bool state)
    {
        Button temp = difficultyButtons[difficulty];
        if(temp != null)
            temp.interactable = state;
    }
}

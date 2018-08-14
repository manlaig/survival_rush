using UnityEngine;

// this script is attached to settings and howToPlay screens
public class SpawnScreenController : MonoBehaviour
{
    public void SpawnScreen()
    {
        RectTransform ui = GameObject.Find("UI").GetComponent<RectTransform>();
        GameObject screen = Instantiate(gameObject, ui);
        screen.GetComponent<RectTransform>().parent = ui;
    }

    public void CloseScreen()
    {
        Destroy(gameObject);
    }
}

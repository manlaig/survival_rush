using UnityEngine;

public class WallColliderPosition : MonoBehaviour
{
    public Transform top, bottom, left, right;
	Camera cam;

	void Start ()
	{
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
        SetWallsPosition();
        SetWallsScale();
	}

    void SetWallsPosition()
    {
        top.position = new Vector3(cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0f, 0f)).x, 0f, -cam.ScreenToWorldPoint(new Vector3(0f, 0f, Screen.height)).z * 8.5f / 10f);
        bottom.position = new Vector3(cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0f, 0f)).x, 0f, cam.ScreenToWorldPoint(new Vector3(0f, 0f, Screen.height)).z);
        left.position = new Vector3(-cam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x, 0f, 0f);
        right.position = new Vector3(cam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x, 0f, 0f);
    }

    void SetWallsScale()
    {
        top.localScale = new Vector3(cam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f, 0.2f);
        bottom.localScale = new Vector3(cam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f, 0.2f);
        left.localScale = new Vector3(0.2f, 1f, cam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).z);
        right.localScale = new Vector3(0.2f, 1f, cam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).z);
    }
}

using UnityEngine;

public class StaticEnemyMover : MonoBehaviour
{
    string direction = "left";

	void Update ()
	{
		if(direction == "left")
            transform.position += new Vector3(0.6f * Time.deltaTime, 0f, 0f);

        if (direction == "right")
            transform.position -= new Vector3(0.6f * Time.deltaTime, 0f, 0f);
	}
}

using UnityEngine;

/// <summary>
///  This script is attached to an enemy object when a wall of enemies is spawned
///  Deleting the game object from here will delete the enemy
/// 
///  This script is responsible for moving the enemy when a wall of enemies is spawned
/// </summary>
public class StaticEnemyMover : MonoBehaviour
{
    public static string direction = "left";

	void Update ()
	{
		if(direction == "left")
            transform.position += new Vector3(Time.deltaTime, 0f, 0f);

        if (direction == "right")
            transform.position -= new Vector3(Time.deltaTime, 0f, 0f);

        // destroy the wall when they reach the other side
        //if(direction == "left" && transform.position >= )
	}
}

using UnityEngine;

/// <summary>
///  This script is attached to an enemy object when a wall of enemies is spawned
///  This script is responsible for moving the enemy when a wall of enemies is spawned
/// </summary>
public class StaticEnemyMover : MonoBehaviour
{
    public static string direction = "left";
    WallColliderPosition walls;

    void Start()
    {
        walls = GameObject.FindGameObjectWithTag("Walls").GetComponent<WallColliderPosition>();
        GetComponent<EnemyAttack>().activateColliders();

        // get all colliders attached, check if each one is a trigger, if  not, then get rid of it
        // we dont want the static enemy to collide with walls
        Collider[] colliders = GetComponents<SphereCollider>();
        foreach (Collider col in colliders)
            if (!col.isTrigger)
                col.enabled = false;
    }

    void Update ()
	{
		if(direction == "left")
            transform.position += new Vector3(Time.deltaTime * 2f, 0f, 0f);

        if (direction == "right")
            transform.position -= new Vector3(Time.deltaTime * 2f, 0f, 0f);

        // destroy the wall when they reach the other side
        if(direction == "left" && transform.position.x > walls.right.position.x + 1f)
            Destroy(gameObject);
	}
}

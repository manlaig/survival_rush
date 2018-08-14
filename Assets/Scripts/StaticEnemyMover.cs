using UnityEngine;
using UnityEngine.AI;

/// <summary>
///  This script is attached to an enemy object when a wall of enemies is spawned
///  This script is responsible for moving the enemy when a wall of enemies is spawned
/// </summary>
public class StaticEnemyMover : MonoBehaviour
{
    public string direction = "";
    public float speed = 2f;
    WallColliderPosition walls;

    void Start()
    {
        walls = GameObject.FindGameObjectWithTag("Walls").GetComponent<WallColliderPosition>();
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<EnemyAttack>().activateColliders();

        // we dont want the static enemy to collide with walls
        Collider[] colliders = GetComponents<SphereCollider>();
        foreach (Collider col in colliders)
            if (!col.isTrigger)
                col.enabled = false;
    }

    void Update ()
	{
        if (direction == "right")
            transform.position += new Vector3(Time.deltaTime * speed, 0f, 0f);

        if (direction == "left")
            transform.position -= new Vector3(Time.deltaTime * speed, 0f, 0f);


        // destroy the wall when they reach the other side
        if(direction == "left" && transform.position.x < walls.left.position.x - 1f)
            Destroy(gameObject);

        if (direction == "right" && transform.position.x > walls.right.position.x + 1f)
            Destroy(gameObject);

        if (direction == "up")
            transform.position += new Vector3(0f, 0f, Time.deltaTime * speed);

        if (direction == "down")
            transform.position -= new Vector3(0f, 0f, Time.deltaTime * speed);


        if (direction == "up" && transform.position.z > walls.top.position.z + 1f)
            Destroy(gameObject);

        if (direction == "down" && transform.position.z < walls.bottom.position.z - 1f)
            Destroy(gameObject);
	}
}

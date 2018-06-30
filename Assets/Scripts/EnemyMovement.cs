using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    GameObject player;
    GameManager gameController;
    float timeSpawned;
    bool lerpingAlpha;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        timeSpawned = Time.time;
        lerpingAlpha = true;
	}
	
	void Update ()
    {
        // fading in the enemy when its spawned
        if(lerpingAlpha)
        {
            float progress = Time.time - timeSpawned;
            if (progress >= 2f)
                lerpingAlpha = false;
            Color color = GetComponent<MeshRenderer>().material.color;
            color.a = 0;
            GetComponent<MeshRenderer>().material.color = Color.Lerp(color, new Color(0.91f, 0.376f, 0.27f, 1f), progress);
        }

        if (player != null)
        {
            if (player.activeInHierarchy && gameController.isGameStarted() && Time.time - timeSpawned >= 1f)
            {
                GetComponent<EnemyAttack>().activateColliders();
                GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
            }
            if (!player.activeInHierarchy)
                stopAgent();
        }
	}

    public void stopAgent()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
    }

    public void setSpeed(float speed)
    {
        GetComponent<NavMeshAgent>().speed = speed;
    }
}

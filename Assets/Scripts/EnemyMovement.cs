using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    GameObject player;
    GameManager gameController;
    NavMeshAgent agent;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update ()
    {
        if (player != null)
        {
            if (player.activeInHierarchy && gameController.isGameStarted())
                agent.SetDestination(player.transform.position);
            if (!player.activeInHierarchy)
                stopAgent();
        }
	}

    public void stopAgent()
    {
        agent.isStopped = true;
    }

    public void setSpeed(float speed)
    {
        //if (agent == null) Debug.Log("The agent is null");
        //agent.speed = speed;
        //I want different speeds based on the difficulty level, but this line is causing an error
        GetComponent<NavMeshAgent>().speed = speed;
    }
}

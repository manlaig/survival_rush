using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    float timeSpawned;
    bool lerpingAlpha, initialization;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        timeSpawned = Time.time;
        lerpingAlpha = true;
        initialization = false;
    }
	
	void Update ()
    {
        // fading in the enemy when its spawned
        if(lerpingAlpha)
        {
            float progress = Time.time - timeSpawned;
            if (progress >= 1f)
            {
                lerpingAlpha = false;
                initialization = true;
                GetComponent<EnemyAttack>().activateColliders();
            }
            Color color = GetComponent<MeshRenderer>().material.color;
            color.a = 0;
            GetComponent<MeshRenderer>().material.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 1f), progress);
        }

        if (player != null)
        {
            if (ShieldWeaponController.shieldActive)
                runAwayFromPlayer();
            else if(initialization)
                agent.SetDestination(player.transform.position);

            if (!player.activeInHierarchy)
                stopAgent();
        }
	}

    void runAwayFromPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < 7f)
        {
            Vector3 pos = transform.position - player.transform.position;
            agent.SetDestination(transform.position + pos);
        }
    }

    public void stopAgent()
    {
        if(GetComponent<NavMeshAgent>() != null)
            GetComponent<NavMeshAgent>().isStopped = true;
    }

    public void setSpeed(float speed)
    {
        if(GetComponent<NavMeshAgent>() != null)
            GetComponent<NavMeshAgent>().speed = speed;
    }
}

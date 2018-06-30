using UnityEngine;

// change this class' name, enemyattack is an inaccurate name
public class EnemyAttack : MonoBehaviour
{
    GameManager gameManager;
    ParticleSystem deathEffect;
    EnemyMovement agent;

    void Start()
    {
        agent = GetComponent<EnemyMovement>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        deathEffect = GetComponent<ParticleSystem>();
    }

	void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Player")
            gameManager.setGameOver();
        
        if (other.gameObject.tag == "Explosive")
        {
            // destroy this if the explosive is currently active/exploding
            destroyThis();
        }
	}

    public void destroyThis()
    {
        GameManager.score += 10;
        if (!deathEffect.isPlaying)
        {
            deathEffect.Play();
            agent.stopAgent();

            GetComponent<MeshRenderer>().enabled = false;
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            Destroy(gameObject, 0.1f);
        }
    }
}
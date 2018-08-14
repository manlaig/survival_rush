using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField] GameObject deathEffect;

    void OnTriggerEnter(Collider other)
	{
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (other.gameObject.tag == "Player")
            gameManager.setGameOver();
        
        if (other.gameObject.tag == "Explosive")
            destroyThis();
	}

    public void destroyThis()
    {
        GameManager.score += 50;

        if (!deathEffect.activeInHierarchy)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(effect, 0.5f);
            Destroy(gameObject);
        }
    }

    public void activateColliders()
    {
        Collider[] colliders = GetComponents<SphereCollider>();
        foreach (Collider col in colliders)
            col.enabled = true;
    }
}
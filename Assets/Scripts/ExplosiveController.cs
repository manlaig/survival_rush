using UnityEngine;

public class ExplosiveController : MonoBehaviour
{
    public GameObject bg;

    bool exploding = false;
    ParticleSystem explosion, explosionChild;

	void Start ()
    {
        explosion = GetComponent<ParticleSystem>();
        explosionChild = GetComponentInChildren<ParticleSystem>();
	}

    void Update()
    {
        if (!explosion.isPlaying && exploding)
        {
            SpawnManager.weaponsActiveCount--;
            GameObject delBg = Instantiate(bg, transform.position, transform.rotation);
            Destroy(delBg, 5f);
            Destroy(gameObject, 5f);
            explosion.Play();
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            exploding = true;
    }
}

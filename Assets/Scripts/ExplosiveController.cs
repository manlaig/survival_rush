using UnityEngine;

public class ExplosiveController : MonoBehaviour
{
    public GameObject bg;

    bool exploding = false;
    ParticleSystem explosion, explosionChild;
    GameObject delBg;  // choose better name

	void Start ()
    {
        explosion = GetComponent<ParticleSystem>();
        explosionChild = GetComponentInChildren<ParticleSystem>();
	}

    void Update()
    {
        if (!explosion.isPlaying && exploding)
        {
            delBg = Instantiate(bg, transform.position, transform.rotation);
            if(explosionChild.isPlaying)
                explosionChild.Stop();
            explosion.Play();
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 5f);
            Destroy(delBg, 5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            exploding = true;
    }
}
